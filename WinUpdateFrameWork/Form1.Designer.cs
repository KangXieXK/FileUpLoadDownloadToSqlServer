
namespace WinUpdateFrameWork
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.uploadPath = new System.Windows.Forms.TextBox();
            this.DownLoadPath = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.cbb_Program = new System.Windows.Forms.ComboBox();
            this.cbb_Version = new System.Windows.Forms.ComboBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(492, 47);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "上传";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btn_Upload_Click);
            // 
            // uploadPath
            // 
            this.uploadPath.Location = new System.Drawing.Point(42, 49);
            this.uploadPath.Name = "uploadPath";
            this.uploadPath.Size = new System.Drawing.Size(421, 21);
            this.uploadPath.TabIndex = 1;
            // 
            // DownLoadPath
            // 
            this.DownLoadPath.Location = new System.Drawing.Point(42, 119);
            this.DownLoadPath.Name = "DownLoadPath";
            this.DownLoadPath.Size = new System.Drawing.Size(421, 21);
            this.DownLoadPath.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(492, 117);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "下载";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btn_DownLoad_Click);
            // 
            // cbb_Program
            // 
            this.cbb_Program.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_Program.FormattingEnabled = true;
            this.cbb_Program.Location = new System.Drawing.Point(42, 85);
            this.cbb_Program.Name = "cbb_Program";
            this.cbb_Program.Size = new System.Drawing.Size(142, 20);
            this.cbb_Program.TabIndex = 4;
            this.cbb_Program.SelectedIndexChanged += new System.EventHandler(this.cbb_Program_SelectedIndexChanged);
            // 
            // cbb_Version
            // 
            this.cbb_Version.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_Version.FormattingEnabled = true;
            this.cbb_Version.Location = new System.Drawing.Point(240, 85);
            this.cbb_Version.Name = "cbb_Version";
            this.cbb_Version.Size = new System.Drawing.Size(223, 20);
            this.cbb_Version.TabIndex = 5;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(492, 12);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(75, 21);
            this.textBox3.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 177);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.cbb_Version);
            this.Controls.Add(this.cbb_Program);
            this.Controls.Add(this.DownLoadPath);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.uploadPath);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "上传程序";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox uploadPath;
        private System.Windows.Forms.TextBox DownLoadPath;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox cbb_Program;
        private System.Windows.Forms.ComboBox cbb_Version;
        private System.Windows.Forms.TextBox textBox3;
    }
}

