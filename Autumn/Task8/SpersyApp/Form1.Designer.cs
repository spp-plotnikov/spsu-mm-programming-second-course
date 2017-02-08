namespace SpersyApp
{
    partial class Form1
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
            this.GetFiltersBtn = new System.Windows.Forms.Button();
            this.FiltersComboBox = new System.Windows.Forms.ComboBox();
            this.ImagePicBox = new System.Windows.Forms.PictureBox();
            this.SelectImageBtn = new System.Windows.Forms.Button();
            this.OpenFile = new System.Windows.Forms.OpenFileDialog();
            this.SendBtn = new System.Windows.Forms.Button();
            this.ProgressPB = new System.Windows.Forms.ProgressBar();
            this.CancelBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ImagePicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // GetFiltersBtn
            // 
            this.GetFiltersBtn.Location = new System.Drawing.Point(12, 12);
            this.GetFiltersBtn.Name = "GetFiltersBtn";
            this.GetFiltersBtn.Size = new System.Drawing.Size(99, 28);
            this.GetFiltersBtn.TabIndex = 0;
            this.GetFiltersBtn.Text = "Get filters";
            this.GetFiltersBtn.UseVisualStyleBackColor = true;
            this.GetFiltersBtn.Click += new System.EventHandler(this.GetFiltersBtn_Click);
            // 
            // FiltersComboBox
            // 
            this.FiltersComboBox.FormattingEnabled = true;
            this.FiltersComboBox.Location = new System.Drawing.Point(117, 12);
            this.FiltersComboBox.Name = "FiltersComboBox";
            this.FiltersComboBox.Size = new System.Drawing.Size(121, 28);
            this.FiltersComboBox.TabIndex = 1;
            this.FiltersComboBox.SelectionChangeCommitted += new System.EventHandler(this.FiltersComboBox_SelectionChangeCommitted);
            // 
            // ImagePicBox
            // 
            this.ImagePicBox.Location = new System.Drawing.Point(13, 100);
            this.ImagePicBox.Name = "ImagePicBox";
            this.ImagePicBox.Size = new System.Drawing.Size(401, 306);
            this.ImagePicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ImagePicBox.TabIndex = 2;
            this.ImagePicBox.TabStop = false;
            // 
            // SelectImageBtn
            // 
            this.SelectImageBtn.Location = new System.Drawing.Point(420, 100);
            this.SelectImageBtn.Name = "SelectImageBtn";
            this.SelectImageBtn.Size = new System.Drawing.Size(151, 44);
            this.SelectImageBtn.TabIndex = 3;
            this.SelectImageBtn.Text = "Select image";
            this.SelectImageBtn.UseVisualStyleBackColor = true;
            this.SelectImageBtn.Click += new System.EventHandler(this.SelectImageBtn_Click);
            // 
            // OpenFile
            // 
            this.OpenFile.Filter = "Bitmap (*.bmp)|*.bmp";
            // 
            // SendBtn
            // 
            this.SendBtn.Location = new System.Drawing.Point(420, 150);
            this.SendBtn.Name = "SendBtn";
            this.SendBtn.Size = new System.Drawing.Size(151, 44);
            this.SendBtn.TabIndex = 4;
            this.SendBtn.Text = "Send";
            this.SendBtn.UseVisualStyleBackColor = true;
            this.SendBtn.Click += new System.EventHandler(this.SendBtn_Click);
            // 
            // ProgressPB
            // 
            this.ProgressPB.Location = new System.Drawing.Point(12, 67);
            this.ProgressPB.Name = "ProgressPB";
            this.ProgressPB.Size = new System.Drawing.Size(402, 27);
            this.ProgressPB.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.ProgressPB.TabIndex = 6;
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(420, 200);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(151, 43);
            this.CancelBtn.TabIndex = 7;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(811, 418);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.ProgressPB);
            this.Controls.Add(this.SendBtn);
            this.Controls.Add(this.SelectImageBtn);
            this.Controls.Add(this.ImagePicBox);
            this.Controls.Add(this.FiltersComboBox);
            this.Controls.Add(this.GetFiltersBtn);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Spersy";
            ((System.ComponentModel.ISupportInitialize)(this.ImagePicBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button GetFiltersBtn;
        private System.Windows.Forms.ComboBox FiltersComboBox;
        private System.Windows.Forms.PictureBox ImagePicBox;
        private System.Windows.Forms.Button SelectImageBtn;
        private System.Windows.Forms.OpenFileDialog OpenFile;
        private System.Windows.Forms.Button SendBtn;
        private System.Windows.Forms.ProgressBar ProgressPB;
        private System.Windows.Forms.Button CancelBtn;
    }
}

