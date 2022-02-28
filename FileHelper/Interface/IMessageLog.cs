using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHelper
{
    public interface IMessageLog
    {
        log4net.ILog Log { get; set; }

        SocketCenter soketcenter { get; set; }
        void InitLog(log4net.ILog log);
        void InitSocket(SocketCenter soketcenter);
        void StartServer();

        void StopServer();
        void SendMessage(IMessageModel message);
        void RecieveMessage(IMessageModel message);
        void ResponseMessage(IMessageModel message);

        void LogInfo(IMessageModel message);

    }
}
