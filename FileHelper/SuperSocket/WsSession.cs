using SuperSocket.SocketBase.Protocol;
using SuperSocket.WebSocket;
using SuperSocket.WebSocket.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHelper
{
    public class WsSession : WebSocketSession<WsSession>
    {
        protected override void OnSessionStarted()
        {
            this.Send("web会话建立成功");
        }

        protected override void HandleUnknownRequest(IWebSocketFragment requestInfo)
        {
            //base.HandleUnknownRequest(requestInfo);
            this.Send("web未知请求，关键字为" + requestInfo.Key);
        }

        protected override void HandleException(Exception e)
        {
            this.Send("web会话报错: {0}", e.Message);

        }

        public string uid { get; set; }
    }
}
