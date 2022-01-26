
namespace WinUpdate
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_Upload = new System.Windows.Forms.Button();
            this.uploadPath = new System.Windows.Forms.TextBox();
            this.DownLoadPath = new System.Windows.Forms.TextBox();
            this.btn_DownLoad = new System.Windows.Forms.Button();
            this.cbb_Program = new System.Windows.Forms.ComboBox();
            this.cbb_Version = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_Upload
            // 
            this.btn_Upload.Location = new System.Drawing.Point(616, 39);
            this.btn_Upload.Name = "btn_Upload";
            this.btn_Upload.Size = new System.Drawing.Size(75, 23);
            this.btn_Upload.TabIndex = 0;
            this.btn_Upload.Text = "上传";
            this.btn_Upload.UseVisualStyleBackColor = true;
            this.btn_Upload.Click += new System.EventHandler(this.btn_Upload_Click);
            // 
            // uploadPath
            // 
            this.uploadPath.Location = new System.Drawing.Point(30, 39);
            this.uploadPath.Name = "uploadPath";
            this.uploadPath.Size = new System.Drawing.Size(580, 23);
            this.uploadPath.TabIndex = 1;
            // 
            // DownLoadPath
            // 
            this.DownLoadPath.Location = new System.Drawing.Point(30, 122);
            this.DownLoadPath.Name = "DownLoadPath";
            this.DownLoadPath.Size = new System.Drawing.Size(580, 23);
            this.DownLoadPath.TabIndex = 3;
            // 
            // btn_DownLoad
            // 
            this.btn_DownLoad.Location = new System.Drawing.Point(616, 122);
            this.btn_DownLoad.Name = "btn_DownLoad";
            this.btn_DownLoad.Size = new System.Drawing.Size(75, 23);
            this.btn_DownLoad.TabIndex = 2;
            this.btn_DownLoad.Text = "下载";
            this.btn_DownLoad.UseVisualStyleBackColor = true;
            this.btn_DownLoad.Click += new System.EventHandler(this.btn_DownLoad_Click);
            // 
            // cbb_Program
            // 
            this.cbb_Program.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_Program.FormattingEnabled = true;
            this.cbb_Program.Location = new System.Drawing.Point(30, 80);
            this.cbb_Program.Name = "cbb_Program";
            this.cbb_Program.Size = new System.Drawing.Size(273, 25);
            this.cbb_Program.TabIndex = 4;
            this.cbb_Program.SelectedIndexChanged += new System.EventHandler(this.cbb_Program_SelectedIndexChanged);
            // 
            // cbb_Version
            // 
            this.cbb_Version.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_Version.FormattingEnabled = true;
            this.cbb_Version.Location = new System.Drawing.Point(328, 80);
            this.cbb_Version.Name = "cbb_Version";
            this.cbb_Version.Size = new System.Drawing.Size(282, 25);
            this.cbb_Version.TabIndex = 4;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(585, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.PasswordChar = '*';
            this.textBox1.Size = new System.Drawing.Size(106, 23);
            this.textBox1.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 192);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cbb_Version);
            this.Controls.Add(this.cbb_Program);
            this.Controls.Add(this.DownLoadPath);
            this.Controls.Add(this.btn_DownLoad);
            this.Controls.Add(this.uploadPath);
            this.Controls.Add(this.btn_Upload);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "上传工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Upload;
        private System.Windows.Forms.TextBox uploadPath;
        private System.Windows.Forms.TextBox DownLoadPath;
        private System.Windows.Forms.ComboBox cbb_Program;
        private System.Windows.Forms.Button btn_DownLoad;
        private System.Windows.Forms.ComboBox cbb_Version;
        private System.Windows.Forms.TextBox textBox1;
    }
}

