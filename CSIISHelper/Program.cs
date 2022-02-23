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
            SocketCenter sc = new SocketCenter();
            sc.StartWsserver(2020);
            sc.Send(new MessageJsonModel() { BussinessID=1,Content="string",Key="key" }, "127.0.0.1", 2020);
            string str = Console.ReadLine();
        }
    }
}
