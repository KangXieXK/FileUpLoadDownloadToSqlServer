using FileUpdate;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinUpdate
{
    static class Program
    {
        private static EventWaitHandle handle;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) 
        {
            string str = AESHelper.Encrypt("Admin1234%^&*");
            string str2 = AESHelper.Decrypt("XinClFAX515hKWlx8elzxA==");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && EventWaitHandle.TryOpenExisting("mutex", out handle))
            {
                MessageBox.Show("程序已经启动，请勿重复打开。");
                return;
            }
            else
            {
                handle = new EventWaitHandle(false, EventResetMode.AutoReset, "mutex");
                if (args == null || args.Length == 0)
                {
                    Application.SetHighDpiMode(HighDpiMode.SystemAware);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
                }
                else
                {
                    string cmd = args[0];
                    if (cmd.ToUpper() == "u" && args.Length > 1)
                    {
                        string connection = Readjson();
                        string uploadPath = args[1];
                        FileUpdateIns fileup = new FileUpdateIns(connection);
                        fileup.UpLoadBaseFolder(uploadPath);
                    }
                    else
                    {
                        if (cmd.ToLower() == "d" && args.Length > 3)
                        {
                            string connection = Readjson();
                            string downloadPath = args[1];
                            string Program = args[2];
                            FileUpdateIns filed = new FileUpdateIns(connection);
                            Thread.Sleep(1000);
                            string version = args[3];
                            filed.DownLoadFolder(downloadPath, Program, version);
                            if (args.Length == 5)
                            {
                                string programNext = args[4];
                                ConsoleExe(new string[] { programNext });
                            }
                        }
                        if (cmd.ToLower() == "dl" && args.Length > 3)
                        {
                            string connection = Readjson();
                            string downloadPath = args[1];
                            string Program = args[2];
                            FileUpdateIns filed = new FileUpdateIns(connection);
                            Thread.Sleep(1000);
                            filed.DownLoadFolder(downloadPath, Program);
                            if (args.Length == 4)
                            {
                                string programNext = args[3];
                                ConsoleExe(new string[] { programNext });
                            }
                        }
                        if (cmd.ToLower() == "down" && args.Length > 3)
                        {
                            string connection = Readjson();
                            string downloadPath = args[1];
                            string Program = args[2];
                            FileUpdateIns filed = new FileUpdateIns(connection);
                            Thread.Sleep(1000);
                            string version = args[3];
                            filed.DownLoadFolder(downloadPath, Program, version);
                            if (args.Length == 5)
                            {
                                string programNext = args[4];
                                ConsoleExe(new string[] { programNext });
                            }
                        }
                    }
                }
                handle.Close();
            }


        }
        public static void ConsoleExe(string[] args)
        {
            Process.Start(args[0]);
        }

        public static string Readjson()
        {
            string jsonfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.json");
            using (System.IO.StreamReader file = System.IO.File.OpenText(jsonfile))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    try
                    {
                        JObject o = (JObject)JToken.ReadFrom(reader);
                        var conection = o["connetction"];
                        string Server = conection["Server"].ToString();
                        var Database = conection["Database"].ToString();
                        var Uid = conection["Uid"].ToString();
                        var Pwd = AESHelper.Decrypt(conection["Pwd"].ToString());
                        string connectionString = string.Format(@"Server={0};Database={1};User Id={2};Password={3};", Server, Database, Uid, Pwd);
                        return connectionString;
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }
        }
    }
}
