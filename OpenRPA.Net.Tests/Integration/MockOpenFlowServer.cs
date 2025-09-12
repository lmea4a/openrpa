using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenRPA.Net;
using OpenRPA.Interfaces;

namespace OpenRPA.Net.Tests.Integration
{
    internal class MockOpenFlowServer : IDisposable
    {
        private readonly HttpListener _listener;
        private readonly string _prefix;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private bool _dropOnFirstUpdate;
        private Task _listenTask;
        public int Port { get; }
        public string Url => $"ws://127.0.0.1:{Port}/ws/";
        public TaskCompletionSource<bool> UpdateReceived { get; } = new TaskCompletionSource<bool>();

        public MockOpenFlowServer(int port, bool dropOnFirstUpdate)
        {
            Port = port;
            _dropOnFirstUpdate = dropOnFirstUpdate;
            _prefix = $"http://127.0.0.1:{Port}/ws/";
            _listener = new HttpListener();
            _listener.Prefixes.Add(_prefix);
        }

        public void Start()
        {
            _listener.Start();
            _listenTask = Task.Run(ListenLoop);
        }

        private async Task ListenLoop()
        {
            try
            {
                while (!_cts.IsCancellationRequested)
                {
                    var ctx = await _listener.GetContextAsync();
                    if (!ctx.Request.IsWebSocketRequest)
                    {
                        ctx.Response.StatusCode = 400; ctx.Response.Close();
                        continue;
                    }
                    var wsctx = await ctx.AcceptWebSocketAsync(null);
                    _ = Task.Run(() => HandleSocket(wsctx.WebSocket));
                }
            }
            catch (Exception)
            {
            }
        }

        private async Task HandleSocket(WebSocket socket)
        {
            var buffer = new byte[8192];
            try
            {
                while (socket.State == WebSocketState.Open && !_cts.IsCancellationRequested)
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), _cts.Token);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "bye", CancellationToken.None);
                        break;
                    }
                    var data = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var msg = JsonConvert.DeserializeObject<SocketShape>(data);
                    if (msg == null) continue;

                    if (msg.command == "ping")
                    {
                        var pong = new SocketShape { id = Guid.NewGuid().ToString(), replyto = msg.id, command = "pong", data = "{}", count = 1, index = 0, priority = 2 };
                        var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(pong));
                        await socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    else if (msg.command == "updateworkitem")
                    {
                        if (_dropOnFirstUpdate)
                        {
                            _dropOnFirstUpdate = false;
                            try { await socket.CloseAsync(WebSocketCloseStatus.EndpointUnavailable, "drop", CancellationToken.None); } catch { }
                            break;
                        }
                        // Echo back a success reply with result
                        var replyObj = new UpdateWorkitemMessage<IWorkitem>();
                        replyObj.result = new OpenRPA.WorkItems.Workitem { _id = Guid.NewGuid().ToString(), name = "done", state = "successful" };
                        var reply = new SocketShape
                        {
                            id = Guid.NewGuid().ToString(),
                            replyto = msg.id,
                            command = "updateworkitem",
                            data = JsonConvert.SerializeObject(replyObj),
                            count = 1,
                            index = 0,
                            priority = 2
                        };
                        var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reply));
                        await socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                        UpdateReceived.TrySetResult(true);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public void Dispose()
        {
            try { _cts.Cancel(); } catch { }
            try { _listener.Stop(); } catch { }
            try { _listener.Close(); } catch { }
        }

        private class SocketShape
        {
            public string id { get; set; }
            public string replyto { get; set; }
            public string command { get; set; }
            public string data { get; set; }
            public int count { get; set; }
            public int index { get; set; }
            public int priority { get; set; } = 2;
        }
    }
}
