using FileHelper;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.Extensions.Logging;
using SAEA.FileSocket;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace CSIISHelper
{
    class Program
    {
        public static ILog log;
        static void Main(string[] args)
        {
            ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            log = LogManager.GetLogger(repository.Name, "NETlog");
            if (log.IsInfoEnabled) log.Info("Application [ConsoleApp] Start");

            // Log a debug message. Test if debug is enabled before
            // attempting to log the message. This is not required but
            // can make running without logging faster.
            if (log.IsDebugEnabled) log.Debug("This is a debug message");

            SocketCenter sc = new SocketCenter();
            sc.log = log;
            sc.StartWsserver(2020);
            sc.Send(new MessageJsonModel()
            {
                BussinessID = 1,
                Content = new FileCheckQuest()
                {
                    ServerPathAbs = false,
                    ServerPath = "test",
                    ClientPath = "testUp",
                    ClientPathAbs = false,
                    listfile = new FileCheck().CheckBaseFolder(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testUp"))
                },
                Key = "FileCheck"


            }, "127.0.0.1", 2020) ;

            string str = Console.ReadLine();

        }
    }
}
