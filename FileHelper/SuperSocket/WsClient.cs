
using SuperSocket.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;

namespace FileHelper
{
    public class WsClient
    {
        public WebsocketClient client { get; set; }

        public WebsocketClient CreateClient(string ip, int port, string extend = "")
        {
            if (client != null)
            {
                client.Dispose();
            }
            var url = new Uri("ws://" + ip + ":" + port.ToString() + "/" + extend);

            client = new WebsocketClient(url);
            client.ReconnectTimeout = null;
            client.ReconnectionHappened.Subscribe(info => { }
                );
           
            client.Start();
            return client;
        }

        public bool Send(string str)
        {
            if (client == null)
            {
                return false;
            }
            else
            {
                client.Send(str);
            }
            return true;
        }
    }
}
