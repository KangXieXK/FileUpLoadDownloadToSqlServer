using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinActiveTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Length == 8)
            {
                string str = new Active().AesEncrypt(this.textBox1.Text, dateTimePicker1.Value);
                this.textBox2.Text = str;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Length == 8 && this.textBox2.TextLength == 17)
            {
                DateTime dt = DateTime.Now;
                bool b = new Active().AesDecrypt(this.textBox1.Text, this.textBox2.Text, out dt);
                if(b)
                {
                    MessageBox.Show("验证成功");
                }
                else
                {
                    MessageBox.Show("验证失败");
                }
            }
        }
    }
}
