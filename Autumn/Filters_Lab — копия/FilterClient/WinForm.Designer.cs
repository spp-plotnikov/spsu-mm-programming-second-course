namespace FilterClient
{
    partial class WinForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SelectFile = new System.Windows.Forms.Button();
            this.FilterList = new System.Windows.Forms.ComboBox();
            this.SendBtn = new System.Windows.Forms.Button();
            this.BeforeFilePath = new System.Windows.Forms.Label();
            this.ServerAdress = new System.Windows.Forms.Label();
            this.ConnectBtn = new System.Windows.Forms.Button();
            this.ServerText = new System.Windows.Forms.TextBox();
            this.PicBefore = new System.Windows.Forms.PictureBox();
            this.PanelBefore = new System.Windows.Forms.Panel();
            this.PanelAfter = new System.Windows.Forms.Panel();
            this.PicAfter = new System.Windows.Forms.PictureBox();
            this.BeforePath = new System.Windows.Forms.Label();
            this.AfterPath = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PicBefore)).BeginInit();
            this.PanelBefore.SuspendLayout();
            this.PanelAfter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicAfter)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(225, 12);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(796, 36);
            this.progressBar.TabIndex = 0;
            // 
            // SelectFile
            // 
            this.SelectFile.Location = new System.Drawing.Point(130, 53);
            this.SelectFile.Name = "SelectFile";
            this.SelectFile.Size = new System.Drawing.Size(75, 23);
            this.SelectFile.TabIndex = 1;
            this.SelectFile.Text = "Open File";
            this.SelectFile.UseVisualStyleBackColor = true;
            this.SelectFile.Click += new System.EventHandler(this.SelectFile_Click);
            // 
            // FilterList
            // 
            this.FilterList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FilterList.FormattingEnabled = true;
            this.FilterList.Location = new System.Drawing.Point(12, 53);
            this.FilterList.Name = "FilterList";
            this.FilterList.Size = new System.Drawing.Size(100, 21);
            this.FilterList.TabIndex = 2;
            // 
            // SendBtn
            // 
            this.SendBtn.Location = new System.Drawing.Point(12, 95);
            this.SendBtn.Name = "SendBtn";
            this.SendBtn.Size = new System.Drawing.Size(75, 23);
            this.SendBtn.TabIndex = 3;
            this.SendBtn.Text = "Send";
            this.SendBtn.UseVisualStyleBackColor = true;
            this.SendBtn.Click += new System.EventHandler(this.SendBtn_Click);
            // 
            // BeforeFilePath
            // 
            this.BeforeFilePath.AutoSize = true;
            this.BeforeFilePath.Location = new System.Drawing.Point(12, 79);
            this.BeforeFilePath.Name = "BeforeFilePath";
            this.BeforeFilePath.Size = new System.Drawing.Size(0, 13);
            this.BeforeFilePath.TabIndex = 4;
            // 
            // ServerAdress
            // 
            this.ServerAdress.AutoSize = true;
            this.ServerAdress.Location = new System.Drawing.Point(12, 35);
            this.ServerAdress.Name = "ServerAdress";
            this.ServerAdress.Size = new System.Drawing.Size(0, 13);
            this.ServerAdress.TabIndex = 5;
            // 
            // ConnectBtn
            // 
            this.ConnectBtn.Location = new System.Drawing.Point(130, 12);
            this.ConnectBtn.Name = "ConnectBtn";
            this.ConnectBtn.Size = new System.Drawing.Size(75, 23);
            this.ConnectBtn.TabIndex = 6;
            this.ConnectBtn.Text = "Connect";
            this.ConnectBtn.UseVisualStyleBackColor = true;
            this.ConnectBtn.Click += new System.EventHandler(this.ConnectBtn_Click);
            // 
            // ServerText
            // 
            this.ServerText.Location = new System.Drawing.Point(12, 12);
            this.ServerText.Name = "ServerText";
            this.ServerText.Size = new System.Drawing.Size(100, 20);
            this.ServerText.TabIndex = 7;
            this.ServerText.Text = "127.0.0.1:13000";
            // 
            // PicBefore
            // 
            this.PicBefore.Location = new System.Drawing.Point(0, 0);
            this.PicBefore.Name = "PicBefore";
            this.PicBefore.Size = new System.Drawing.Size(200, 200);
            this.PicBefore.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PicBefore.TabIndex = 8;
            this.PicBefore.TabStop = false;
            // 
            // PanelBefore
            // 
            this.PanelBefore.AutoScroll = true;
            this.PanelBefore.Controls.Add(this.PicBefore);
            this.PanelBefore.Location = new System.Drawing.Point(15, 124);
            this.PanelBefore.Name = "PanelBefore";
            this.PanelBefore.Size = new System.Drawing.Size(500, 500);
            this.PanelBefore.TabIndex = 9;
            // 
            // PanelAfter
            // 
            this.PanelAfter.AutoScroll = true;
            this.PanelAfter.Controls.Add(this.PicAfter);
            this.PanelAfter.Location = new System.Drawing.Point(521, 124);
            this.PanelAfter.Name = "PanelAfter";
            this.PanelAfter.Size = new System.Drawing.Size(500, 500);
            this.PanelAfter.TabIndex = 10;
            // 
            // PicAfter
            // 
            this.PicAfter.Location = new System.Drawing.Point(0, 0);
            this.PicAfter.Name = "PicAfter";
            this.PicAfter.Size = new System.Drawing.Size(200, 200);
            this.PicAfter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PicAfter.TabIndex = 9;
            this.PicAfter.TabStop = false;
            // 
            // BeforePath
            // 
            this.BeforePath.AutoSize = true;
            this.BeforePath.Location = new System.Drawing.Point(130, 104);
            this.BeforePath.Name = "BeforePath";
            this.BeforePath.Size = new System.Drawing.Size(0, 13);
            this.BeforePath.TabIndex = 12;
            // 
            // AfterPath
            // 
            this.AfterPath.AutoSize = true;
            this.AfterPath.Location = new System.Drawing.Point(521, 104);
            this.AfterPath.Name = "AfterPath";
            this.AfterPath.Size = new System.Drawing.Size(0, 13);
            this.AfterPath.TabIndex = 13;
            // 
            // WinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1041, 640);
            this.Controls.Add(this.AfterPath);
            this.Controls.Add(this.BeforePath);
            this.Controls.Add(this.PanelAfter);
            this.Controls.Add(this.PanelBefore);
            this.Controls.Add(this.ServerText);
            this.Controls.Add(this.ConnectBtn);
            this.Controls.Add(this.ServerAdress);
            this.Controls.Add(this.BeforeFilePath);
            this.Controls.Add(this.SendBtn);
            this.Controls.Add(this.FilterList);
            this.Controls.Add(this.SelectFile);
            this.Controls.Add(this.progressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "WinForm";
            this.Text = "BMP Filter Client";
            ((System.ComponentModel.ISupportInitialize)(this.PicBefore)).EndInit();
            this.PanelBefore.ResumeLayout(false);
            this.PanelBefore.PerformLayout();
            this.PanelAfter.ResumeLayout(false);
            this.PanelAfter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicAfter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button SelectFile;
        private System.Windows.Forms.ComboBox FilterList;
        private System.Windows.Forms.Button SendBtn;
        private System.Windows.Forms.Label BeforeFilePath;
        private System.Windows.Forms.Label ServerAdress;
        private System.Windows.Forms.Button ConnectBtn;
        private System.Windows.Forms.TextBox ServerText;
        private System.Windows.Forms.PictureBox PicBefore;
        private System.Windows.Forms.Panel PanelBefore;
        private System.Windows.Forms.Panel PanelAfter;
        private System.Windows.Forms.PictureBox PicAfter;
        private System.Windows.Forms.Label BeforePath;
        private System.Windows.Forms.Label AfterPath;
    }
}