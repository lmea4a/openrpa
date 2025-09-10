using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenRPA.Net;
using OpenRPA.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OpenRPA.Net.Tests
{
    [TestClass]
    public class WebSocketClientOfflineTests
    {
        [TestMethod]
        public async Task UpdateWorkitem_WhenOffline_QueuesPendingUpdate()
        {
            // Arrange: unique workitem id and ensure clean pending folder
            var client = WebSocketClient.Get("wss://unittest.local/");
            var workitemId = Guid.NewGuid().ToString();
            var wi = new OpenRPA.WorkItems.Workitem
            {
                _id = workitemId,
                name = "unittest",
                state = "successful"
            };

            // Reflect the private GetPendingUpdatesPath to locate folder
            var mi = typeof(WebSocketClient).GetMethod("GetPendingUpdatesPath", BindingFlags.NonPublic | BindingFlags.Instance);
            var pendingPath = (string)mi.Invoke(client, null);
            if (Directory.Exists(pendingPath))
            {
                foreach (var f in Directory.GetFiles(pendingPath)) File.Delete(f);
            }
            else
            {
                Directory.CreateDirectory(pendingPath);
            }

            // Act: simulate offline by not connecting and attempt to update
            var result = await client.UpdateWorkitem<IWorkitem>(wi, null, ignoremaxretries: false, traceId: "", spanId: "");

            // Assert: result falls back to input and a pending file exists
            Assert.IsNotNull(result);
            Assert.AreEqual(workitemId, result._id);
            var queued = Directory.GetFiles(pendingPath, workitemId + "-*.json");
            Assert.IsTrue(queued.Any(), "Expected a queued pending update file for the workitem");
        }
    }
}

