using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OpenRPA.Net;
using OpenRPA.Interfaces;

namespace OpenRPA.Net.Tests.Integration
{
    [TestClass]
    public class OpenCoreReconnectPendingIT
    {
        private static string GetEnv(string key) => Environment.GetEnvironmentVariable(key);

        [TestMethod]
        [TestCategory("External")]
        public async Task Connect_OpenCore_Drop_During_Update_Flush_On_Reconnect()
        {
            var wsurl = GetEnv("OPENRPA_WS_URL");
            var jwt = GetEnv("OPENRPA_JWT");
            var wiq = GetEnv("OPENRPA_WIQ");
            var wiqid = GetEnv("OPENRPA_WIQID");
            var addItem = string.Equals(GetEnv("OPENRPA_ADD_WORKITEM"), "true", StringComparison.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(wsurl) || string.IsNullOrWhiteSpace(jwt) ||
                (string.IsNullOrWhiteSpace(wiq) && string.IsNullOrWhiteSpace(wiqid)))
            {
                Assert.Inconclusive("Environment not configured: set OPENRPA_WS_URL, OPENRPA_JWT, and OPENRPA_WIQ or OPENRPA_WIQID to run this test.");
                return;
            }

            // Configure client
            OpenRPA.Config.local.wsurl = wsurl;
            OpenRPA.Config.local.network_message_timeout = TimeSpan.FromSeconds(3);
            var client = WebSocketClient.Get(wsurl);

            // Clean pending folder
            var mi = typeof(WebSocketClient).GetMethod("GetPendingUpdatesPath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var pendingPath = (string)mi.Invoke(client, null);
            if (Directory.Exists(pendingPath)) foreach (var f in Directory.GetFiles(pendingPath)) File.Delete(f);
            else Directory.CreateDirectory(pendingPath);

            await client.Connect();
            await client.Signin(jwt);

            // Ensure there is a workitem to process
            if (addItem)
            {
                var newItem = new OpenRPA.WorkItems.Workitem
                {
                    name = "it-reconnect-test",
                    wiq = wiq,
                    wiqid = wiqid,
                    state = "processing"
                };
                await client.AddWorkitem<IWorkitem>(newItem, null, "", "");
            }

            // Pop workitem from queue
            var popped = await client.PopWorkitem<IWorkitem>(wiq, wiqid, "", "");
            Assert.IsNotNull(popped, "No workitem retrieved from queue.");

            // Simulate connection drop just before update
            await client.Close();

            // Attempt to update (should be queued to pending)
            popped.state = "successful";
            var result = await client.UpdateWorkitem<IWorkitem>(popped, null, false, "", "");
            Assert.IsNotNull(result);

            // Verify pending file created
            var pattern = string.IsNullOrEmpty(popped._id) ? "*.json" : popped._id + "-*.json";
            var queued = Directory.GetFiles(pendingPath, pattern);
            Assert.IsTrue(queued.Any(), "Pending update was not created after offline update.");

            // Reconnect to flush pending updates
            await client.Connect();

            // Wait for flush (pending file should be deleted)
            var waited = false;
            for (int i = 0; i < 20; i++)
            {
                await Task.Delay(500);
                if (!Directory.GetFiles(pendingPath, pattern).Any()) { waited = true; break; }
            }
            Assert.IsTrue(waited, "Pending update file was not removed after reconnect/flush.");
        }
    }
}

