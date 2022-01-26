using FileUpdateFrameWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WinDownLoadFrameWork
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //string str = AESHelper.Encrypt("clims1.0");
            //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && EventWaitHandle.TryOpenExisting("mutex", out handle))
            //{
            //    MessageBox.Show("程序已经启动，请勿重复打开。");
            //    return;
            //}
            //else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                if (args == null || args.Length == 0)
                {
                    return;
                }
                else
                {
                    string cmd = args[0];
                    if (cmd.ToLower() == "d" && args.Length > 3)
                    {
                        string[] connectionparms = args[1].Split(";".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                        connectionparms[3] = AESHelper.Decrypt(connectionparms[3]);
                        string connection = string.Format("Data Source={0};Database={1};User Id={2};Password={3};", connectionparms);
                        string downloadPath = args[2];
                        string Program = args[3];
                        string version = args[4];
                        FileUpdateIns filed = new FileUpdateIns(connection);
                        downloadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app");
                        var  list = filed.CheckNeedList(downloadPath, Program, version);
                        if(list?.Count>0)
                        {
                            Form2 f = new Form2();
                            try
                            {
                                f.fui = filed;
                                f.DownLoadPath = downloadPath;
                                f.cbb_Program = Program;
                                f.cbb_Version = version;
                                f.list = list;
                                Application.Run(f);
                            }
                            catch
                            {
                                f.Close();
                            }
                            finally
                            {
                                
                            }
                        }
                        if (args.Length >= 6)
                        {
                            string programNext = Path.Combine(downloadPath, Program, version, args[5]);
                            string str = "DOWNLOAD";
                            if (args.Length == 7)
                            {
                                str = args[6];
                            }
                            ConsoleExe(new string[] { programNext }, str);
                        }
                    }
                }

            }

        }
        public static void ConsoleExe(string[] args, string nextarg)
        {
            Process.Start(args[0], nextarg);
        }
    }
}

