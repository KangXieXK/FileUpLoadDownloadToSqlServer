using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinUpdateFrameWork
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string connectionString = "";
        private FileUpdateFrameWork.FileUpdateIns fui;
        private void Form1_Load(object sender, EventArgs e)
        {
            this.uploadPath.Text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UpLoad");
            this.DownLoadPath.Text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DownLoad");
            Readjson();
            fui = new FileUpdateFrameWork.FileUpdateIns(this.connectionString);
            this.cbb_Program.DataSource = fui.GetCanDownLoadProgram();
        }


        public string Readjson()
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
                        var Pwd = FileUpdateFrameWork.AESHelper.Decrypt(conection["Pwd"].ToString());
                        var lastUploadPath = o["LastUploadPath"]?.ToString();
                        var lastDownPath = o["LastDownPath"]?.ToString();

                        this.connectionString = string.Format(@"Server={0};Database={1};User Id={2};Password={3};", Server, Database, Uid, Pwd);
                        return connectionString;
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }
        }

        private void btn_Upload_Click(object sender, EventArgs e)
        {
            if ((this.textBox3.Text == "CSKY") && !string.IsNullOrEmpty(this.uploadPath.Text))
            {
                if (!Directory.Exists(this.uploadPath.Text))
                {
                    MessageBox.Show("文件夹不存在");
                    return;
                }
                fui.UpLoadBaseFolder(this.uploadPath.Text);
                MessageBox.Show("上传成功");
            }
            else
            {
                MessageBox.Show("密码错误");
            }
        }

        private void cbb_Program_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cbb_Version.DataSource = fui.GetCanDownLoadProgramVersion(this.cbb_Program.SelectedItem.ToString());
        }

        private void btn_DownLoad_Click(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(this.DownLoadPath.Text))
            //{
            //    if (!Directory.Exists(this.DownLoadPath.Text))
            //    {
            //        Directory.CreateDirectory(this.DownLoadPath.Text);
            //    }
            //    if (!string.IsNullOrEmpty(this.cbb_Program.Text) && !string.IsNullOrEmpty(this.cbb_Version.Text))
            //    {
            //        string path = Path.Combine(this.DownLoadPath.Text, this.cbb_Program.Text, this.cbb_Version.Text);
            //        fui.DownLoadFolder(path, this.cbb_Program.Text, this.cbb_Version.Text);
            //    }
            //}
            Form2 form2 = new Form2();
            form2.fui = this.fui;
            form2.cbb_Program = this.cbb_Program.Text;
            form2.cbb_Version = this.cbb_Version.Text;
            form2.DownLoadPath = this.DownLoadPath.Text;
            form2.Show();
        }
    }
}
