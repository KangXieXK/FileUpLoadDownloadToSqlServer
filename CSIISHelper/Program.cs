using FileHelper;
using SAEA.FileSocket;
using SAEA.WebSocket.Model;
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

            socketCenter = new SocketCenter();
            if (args.Length > 0)
            {
                int port = int.Parse(args[0]);
                socketCenter.StartWsserver(port);
                string path = args[1];
                socketCenter.FileStorePath = path;
                if (args.Length > 2)
                {
                    string ip = args[2];
                    string localpath = args[3];
                    socketCenter.UploadFileListFirstStep(localpath, ip, port);
                }
            }
            string str = Console.ReadLine();
        }



        static SocketCenter socketCenter;
       
    }
}
