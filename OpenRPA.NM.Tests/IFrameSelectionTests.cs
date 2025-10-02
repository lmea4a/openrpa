using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenRPA.NM;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenRPA.NM.Tests
{
    [TestClass]
    public class IFrameSelectionTests
    {
        private CancellationTokenSource _cts;
        private Task _serverTask;
        private int _port;
        private string _baseUrl;

        [TestInitialize]
        public void SetUp()
        {
            _cts = new CancellationTokenSource();
            _port = GetFreeTcpPort();
            _baseUrl = $"http://127.0.0.1:{_port}";
            _serverTask = RunServerAsync(_port, _cts.Token);
        }

        [TestCleanup]
        public void TearDown()
        {
            try { _cts.Cancel(); } catch { }
            try { _serverTask?.Wait(TimeSpan.FromSeconds(5)); } catch { }
        }

        [TestMethod]
        [TestCategory("Integration"), TestCategory("NM"), TestCategory("Iframe")]
        public void AnchorBased_ElementsInsideIframe_AreFound()
        {
            NMHook.checkForPipes(true, true, true);
            if (!WaitUntil(() => NMHook.connected, TimeSpan.FromSeconds(15)))
            {
                Assert.Inconclusive("NM addon not connected. Install/enable the browser extension and rerun.");
            }

            var parentUrl = _baseUrl + "/parent.html";
            NMHook.openurl("chrome", parentUrl, true, string.Empty, string.Empty);

            // Wait for target tab to be ready
            if (!WaitUntil(() => NMHook.FindTabByURL("chrome", parentUrl) != null, TimeSpan.FromSeconds(10)))
            {
                Assert.Inconclusive("Tab did not open or extension did not report it.");
            }

            // 1) Find the iframe element
            var findFrameSel = "[ {\"Selector\":\"NM\",\"browser\":\"chrome\"}, {\"xpath\":\"//iframe[@id='testframe']\"} ]";
            var frames = NMSelector.GetElementsWithuiSelector(new NMSelector(findFrameSel), null, 1);
            Assert.IsNotNull(frames, "Frames array is null");
            Assert.IsTrue(frames.Length >= 1, "Iframe not found on parent page");

            // 2) Find inner element inside iframe using From=frame
            var innerSel = "[ {\"cssselector\":\"#inner\"} ]";
            var inner = NMSelector.GetElementsWithuiSelector(new NMSelector(innerSel), frames[0], 1);
            Assert.IsTrue(inner.Length >= 1, "Inner element inside iframe not found using anchor-based selection");
        }

        [TestMethod]
        [TestCategory("Integration"), TestCategory("NM"), TestCategory("Iframe")]
        public void RootFrameProperty_ElementsInsideIframe_AreFound()
        {
            NMHook.checkForPipes(true, true, true);
            if (!WaitUntil(() => NMHook.connected, TimeSpan.FromSeconds(15)))
            {
                Assert.Inconclusive("NM addon not connected. Install/enable the browser extension and rerun.");
            }

            var parentUrl = _baseUrl + "/parent.html";
            NMHook.openurl("chrome", parentUrl, true, string.Empty, string.Empty);
            if (!WaitUntil(() => NMHook.FindTabByURL("chrome", parentUrl) != null, TimeSpan.FromSeconds(10)))
            {
                Assert.Inconclusive("Tab did not open or extension did not report it.");
            }

            // First, find an element inside iframe using anchor to get its frameId
            var findFrameSel = "[ {\"Selector\":\"NM\",\"browser\":\"chrome\"}, {\"xpath\":\"//iframe[@id='testframe']\"} ]";
            var frames = NMSelector.GetElementsWithuiSelector(new NMSelector(findFrameSel), null, 1);
            Assert.IsTrue(frames.Length >= 1, "Iframe not found on parent page");
            var innerSel = "[ {\"cssselector\":\"#inner\"} ]";
            var inner = NMSelector.GetElementsWithuiSelector(new NMSelector(innerSel), frames[0], 1);
            Assert.IsTrue(inner.Length >= 1, "Inner element inside iframe not found (anchor step)");

            var frameId = inner[0].message.frameId;
            Assert.IsTrue(frameId > 0, "frameId for inner element not set");

            // Now use root frame property without anchor
            var withoutAnchorSel = $"[ {{\"Selector\":\"NM\",\"browser\":\"chrome\",\"frame\":\"{frameId}\"}}, {{\"cssselector\":\"#inner\"}} ]";
            var inner2 = NMSelector.GetElementsWithuiSelector(new NMSelector(withoutAnchorSel), null, 1);
            Assert.IsTrue(inner2.Length >= 1, "Inner element inside iframe not found using root frame property");
        }

        private static bool WaitUntil(Func<bool> condition, TimeSpan timeout)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            while (sw.Elapsed < timeout)
            {
                if (condition()) return true;
                Thread.Sleep(100);
            }
            return false;
        }

        private static int GetFreeTcpPort()
        {
            var l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }

        private static async Task RunServerAsync(int port, CancellationToken cancellationToken)
        {
            var listener = new HttpListener();
            var prefix = $"http://+:{port}/";
            listener.Prefixes.Add(prefix);
            try
            {
                listener.Start();
            }
            catch (HttpListenerException)
            {
                // Fallback to localhost only if wildcard is not allowed
                listener = new HttpListener();
                listener.Prefixes.Add($"http://127.0.0.1:{port}/");
                listener.Prefixes.Add($"http://localhost:{port}/");
                listener.Start();
            }

            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var parentPath = Path.Combine(baseDir, "Parent.html");
            var childPath = Path.Combine(baseDir, "Child.html");
            if (!File.Exists(parentPath)) parentPath = Path.Combine(baseDir, "Resources", "Parent.html");
            if (!File.Exists(childPath)) childPath = Path.Combine(baseDir, "Resources", "Child.html");

            while (!cancellationToken.IsCancellationRequested)
            {
                HttpListenerContext ctx = null;
                try
                {
                    ctx = await listener.GetContextAsync();
                }
                catch (ObjectDisposedException) { break; }
                catch (HttpListenerException) { break; }
                catch (Exception)
                {
                    if (cancellationToken.IsCancellationRequested) break;
                }
                if (ctx == null) continue;
                try
                {
                    string path = ctx.Request.Url.AbsolutePath;
                    string content = "";
                    string contentType = "text/html; charset=utf-8";
                    if (path.Equals("/parent.html", StringComparison.OrdinalIgnoreCase))
                    {
                        content = File.ReadAllText(parentPath, Encoding.UTF8);
                    }
                    else if (path.Equals("/child.html", StringComparison.OrdinalIgnoreCase))
                    {
                        content = File.ReadAllText(childPath, Encoding.UTF8);
                    }
                    else
                    {
                        content = "<html><body><a href=\"/parent.html\">parent</a></body></html>";
                    }
                    var buffer = Encoding.UTF8.GetBytes(content);
                    ctx.Response.ContentType = contentType;
                    ctx.Response.ContentLength64 = buffer.Length;
                    await ctx.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
                    ctx.Response.Close();
                }
                catch
                {
                    try { ctx.Response.Abort(); } catch { }
                }
            }
            try { listener.Stop(); listener.Close(); } catch { }
        }
    }
}

