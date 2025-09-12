using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenRPA.Net.Tests.Integration;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OpenRPA.Net;
using OpenRPA.Interfaces;

namespace OpenRPA.Net.Tests.Integration
{
    [TestClass]
    public class ReconnectAndPendingIntegrationTests
    {
        private static int FindFreePort()
        {
            var r = new Random();
            for (int i = 0; i < 20; i++)
            {
                int p = r.Next(20000, 40000);
                try
                {
                    var listener = new HttpListener();
                    listener.Prefixes.Add($"http://127.0.0.1:{p}/ws/");
                    listener.Start();
                    listener.Stop();
                    listener.Close();
                    return p;
                }
                catch { }
            }
            return 30888;
        }

        [TestMethod]
        public async Task PendingUpdate_Flushes_After_Reconnect()
        {
            // Speed up timeouts
            OpenRPA.Config.local.network_message_timeout = TimeSpan.FromSeconds(1);

            var port = FindFreePort();
            using (var server1 = new MockOpenFlowServer(port, dropOnFirstUpdate: true))
            {
                server1.Start();
                var url = server1.Url;
                OpenRPA.Config.local.wsurl = url;

                var client = WebSocketClient.Get(url);

                // Clean pending folder
                var mi = typeof(WebSocketClient).GetMethod("GetPendingUpdatesPath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var pendingPath = (string)mi.Invoke(client, null);
                if (Directory.Exists(pendingPath)) foreach (var f in Directory.GetFiles(pendingPath)) File.Delete(f);
                else Directory.CreateDirectory(pendingPath);

                await client.Connect();

                var wi = new OpenRPA.WorkItems.Workitem
                {
                    _id = Guid.NewGuid().ToString(),
                    name = "itest",
                    state = "successful"
                };

                // This update will experience a dropped connection, and should be queued
                var _ = await client.UpdateWorkitem<IWorkitem>(wi, null, false, "", "");

                var queued = Directory.GetFiles(pendingPath, wi._id + "-*.json");
                Assert.IsTrue(queued.Any(), "Pending update was not created");

                // Bring server back up to accept reconnection and flush pending
                using (var server2 = new MockOpenFlowServer(port, dropOnFirstUpdate: false))
                {
                    server2.Start();
                    // trigger reconnect and flush
                    await client.Connect();
                    // wait for server to receive the flushed update
                    var ok = await Task.WhenAny(server2.UpdateReceived.Task, Task.Delay(TimeSpan.FromSeconds(10))) == server2.UpdateReceived.Task;
                    Assert.IsTrue(ok, "Server did not receive flushed update");

                    // pending file should be deleted after successful flush
                    await Task.Delay(500); // small delay for file delete
                    var stillQueued = Directory.GetFiles(pendingPath, wi._id + "-*.json");
                    Assert.IsFalse(stillQueued.Any(), "Pending update file not deleted after flush");
                }
            }
        }
    }
}
