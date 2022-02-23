using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SuperSocket;
using SuperSocket.ProtoBase;
using SuperSocket.WebSocket;
using SuperSocket.WebSocket.Server;
namespace FileHelper
{
    public class WsServer
    {
        public IHost Host { get; set; }
        public IServer Server { get; set; }

        public List<WebSocketSession> SessionList { get; set; }
        public Action<WebSocketSession, WebSocketPackage> MessageAction { get; set; }
        public async Task Start()
        {
            SessionList = new List<WebSocketSession>();
            Host = WebSocketHostBuilder.Create()
                        .UseWebSocketMessageHandler(
                            async (session, message) =>
                            {
                                await Task.Run(() => { if (MessageAction != null) MessageAction(session, message); });
                            }
                        )
                        .ConfigureLogging((hostCtx, loggingBuilder) =>
                        {
                            loggingBuilder.AddConsole();
                        })
                        .Build();
            Server = Host.AsServer();
            await Host.RunAsync();
        }

        public async Task Stop()
        {
            
            await Host.StopAsync();
        }
    }
}
