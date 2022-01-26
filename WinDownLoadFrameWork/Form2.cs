using FileUpdateFrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinDownLoadFrameWork
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public FileUpdateIns fui;
        public string cbb_Version;
        public string cbb_Program;
        public string DownLoadPath;
        public List<SYS_FILEVERSION> list;
        public void DownLoad(string path, string program, string vesion, List<SYS_FILEVERSION> list)
        {
            fui.ActionForCount = null;
            fui.ActionForMes = null;
            fui.ActionForFinish = null;
            fui.ActionForCount += new Action<int, int>(Count);
            fui.ActionForMes += new Action<string>((str) => { this.BeginInvoke(new Action(() => { this.label1.Text = str; })); });
            fui.ActionForFinish += new Action<string>((str) => this.BeginInvoke(new Action(() => { Thread.Sleep(1000); this.Close(); })));
            if (!string.IsNullOrEmpty(path))
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (!string.IsNullOrEmpty(program) && !string.IsNullOrEmpty(vesion))
                {
                    string path2 = Path.Combine(path, program, vesion);
                    try
                    {
                        Task.Factory.StartNew(() => { fui.StartDownload(path2, program, vesion, list); Thread.Sleep(1000); });
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {

                    }
                }
            }
        }

        public void Count(int i, int j)
        {
            if (i > 0 && j > 0)
            {
                this.BeginInvoke(new Action(() => { this.progressBar1.Maximum = j; this.progressBar1.Value = i; }));
            }
            if (j == 0)
            {
                this.BeginInvoke(new Action(() => { this.progressBar1.Maximum = 0; this.progressBar1.Value = 0; }));

            }

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            DownLoad(DownLoadPath, cbb_Program, cbb_Version, list);
        }
    }
}
