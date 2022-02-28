using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHelper
{
    public class FileLog : IMessageLog
    {
        public ILog Log { get; set; }
        public SocketCenter soketcenter { get; set; }

        public void InitLog(ILog log)
        {
            this.Log = log;
        }

        public void InitSocket(SocketCenter sc)
        {
            soketcenter = sc;
        }

        public void RecieveMessage(IMessageModel message)
        {
            Log.Info("收到消息");
        }

        public void ResponseMessage(IMessageModel message)
        {
            
        }

        public void SendMessage(IMessageModel message)
        {
            
        }

        public void StartServer()
        {
            Log.Info("服务启动成功，开始监听"+this.soketcenter.Server.Host);
        }

        public void StopServer()
        {
            Log.Info("服务停止");
        }

        public void LogInfo(IMessageModel message)
        {
            //Log.Error(Newtonsoft.Json.JsonConvert.SerializeObject( message, Formatting.Indented));
        }
    }
}
