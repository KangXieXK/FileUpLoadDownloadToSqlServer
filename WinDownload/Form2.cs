using FileUpdate;
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

namespace WinDownload
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public FileUpdateIns fui;
        public string path { get; set; }
        public string program { get; set; }
        public string vesion { get; set; }

        public void DownLoad(string path, string program, string vesion)
        {
            fui.ActionForCount += new Action<int, int>(Count);
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
                        { fui.DownLoadFolder(path2, program, vesion); };
                    }
                    catch
                    {

                    }
                    finally
                    {
                        this.Close();
                    }
                }
            }
        }

        public void Count(int i, int j)
        {
            if (i > 0 && j > 0)
            {
                this.Invoke(new Action(() => { this.progressBar1.Value = (int)Math.Floor((decimal)i * 100 / j); }));
            }
            if (j == 0)
            {
                this.Invoke(new Action(() => { this.progressBar1.Value = 100; }));
            }

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            DownLoad(path, program, vesion);
        }
    }
}
