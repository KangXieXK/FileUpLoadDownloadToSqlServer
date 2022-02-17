using FileHelper;
using SAEA.FileSocket;
using System;
using System.Text;
using System.Threading;

namespace CSIISHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            //FileHelper.FileMaster fileMaster = new FileHelper.FileMaster();
            //fileMaster.StartReceive(args[0]);
            //if (args.Length > 1)
            //{
            //    string filename = args[1];
            //    string ip = args[2];
            //    string str = Console.ReadLine();
            //    fileMaster.Send(filename, ip);
            //}
            //Console.ReadLine();

            WsServer ws = new WsServer();
            ws.NewMessageReceived += Ws_NewMessageReceived; ;//当有信息传入时
            ws.NewSessionConnected += Ws_NewSessionConnected; ;//当有用户连入时
            ws.SessionClosed += Ws_SessionClosed; ;//当有用户退出时
            ws.NewDataReceived += Ws_NewDataReceived; ;//当有数据传入时
            if (ws.Setup(10086))//绑定端口
                ws.Start();//启动服务  
            string str = Console.ReadLine();
        }

        private static void Ws_NewDataReceived(WsSession session, byte[] value)
        {
            Console.WriteLine(System.Text.Encoding.UTF8.GetString(value));
        }

        private static void Ws_SessionClosed(WsSession session, SuperSocket.SocketBase.CloseReason value)
        {
            Console.WriteLine(value.ToString());
        }


        private static void Ws_NewMessageReceived(WsSession session, string value)
        {
            Console.WriteLine(value);
        }

        private static void Ws_NewSessionConnected(WsSession session)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(session));
        }

        static SocketCenter socketCenter;

    }
}
