namespace Client
{
    partial class ClientInterface
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
            this.AddPictureButton = new System.Windows.Forms.Button();
            this.SendPictureButton = new System.Windows.Forms.Button();
            this.CancelWorkButton = new System.Windows.Forms.Button();
            this.filterBox = new System.Windows.Forms.GroupBox();
            this.newPictureBox = new System.Windows.Forms.PictureBox();
            this.oldPictureBox = new System.Windows.Forms.PictureBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.percent = new System.Windows.Forms.Label();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.newPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.oldPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // AddPictureButton
            // 
            this.AddPictureButton.Location = new System.Drawing.Point(251, 363);
            this.AddPictureButton.Name = "AddPictureButton";
            this.AddPictureButton.Size = new System.Drawing.Size(113, 37);
            this.AddPictureButton.TabIndex = 0;
            this.AddPictureButton.Text = "Add Picture";
            this.AddPictureButton.UseVisualStyleBackColor = true;
            this.AddPictureButton.Click += new System.EventHandler(this.AddPictureButton_Click);
            // 
            // SendPictureButton
            // 
            this.SendPictureButton.Location = new System.Drawing.Point(493, 363);
            this.SendPictureButton.Name = "SendPictureButton";
            this.SendPictureButton.Size = new System.Drawing.Size(113, 37);
            this.SendPictureButton.TabIndex = 1;
            this.SendPictureButton.Text = "Send Picture";
            this.SendPictureButton.UseVisualStyleBackColor = true;
            this.SendPictureButton.Click += new System.EventHandler(this.SendPictureButton_Click);
            // 
            // CancelWorkButton
            // 
            this.CancelWorkButton.Location = new System.Drawing.Point(733, 363);
            this.CancelWorkButton.Name = "CancelWorkButton";
            this.CancelWorkButton.Size = new System.Drawing.Size(113, 37);
            this.CancelWorkButton.TabIndex = 2;
            this.CancelWorkButton.Text = "Cancel";
            this.CancelWorkButton.UseVisualStyleBackColor = true;
            this.CancelWorkButton.Click += new System.EventHandler(this.CancelWorkButton_Click);
            // 
            // filterBox
            // 
            this.filterBox.Location = new System.Drawing.Point(24, 12);
            this.filterBox.Name = "filterBox";
            this.filterBox.Size = new System.Drawing.Size(212, 388);
            this.filterBox.TabIndex = 3;
            this.filterBox.TabStop = false;
            this.filterBox.Text = "Filters";
            // 
            // newPictureBox
            // 
            this.newPictureBox.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.newPictureBox.Location = new System.Drawing.Point(563, 12);
            this.newPictureBox.Name = "newPictureBox";
            this.newPictureBox.Size = new System.Drawing.Size(283, 294);
            this.newPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.newPictureBox.TabIndex = 0;
            this.newPictureBox.TabStop = false;
            // 
            // oldPictureBox
            // 
            this.oldPictureBox.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.oldPictureBox.Location = new System.Drawing.Point(251, 12);
            this.oldPictureBox.Name = "oldPictureBox";
            this.oldPictureBox.Size = new System.Drawing.Size(283, 294);
            this.oldPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.oldPictureBox.TabIndex = 1;
            this.oldPictureBox.TabStop = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(251, 319);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(595, 32);
            this.progressBar.TabIndex = 4;
            // 
            // percent
            // 
            this.percent.BackColor = System.Drawing.Color.Transparent;
            this.percent.Location = new System.Drawing.Point(418, 324);
            this.percent.Name = "percent";
            this.percent.Size = new System.Drawing.Size(256, 23);
            this.percent.TabIndex = 5;
            this.percent.Text = "0%";
            this.percent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.StartWork);
            // 
            // ClientInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 412);
            this.Controls.Add(this.percent);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.newPictureBox);
            this.Controls.Add(this.oldPictureBox);
            this.Controls.Add(this.filterBox);
            this.Controls.Add(this.CancelWorkButton);
            this.Controls.Add(this.SendPictureButton);
            this.Controls.Add(this.AddPictureButton);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ClientInterface";
            this.Text = "Client";
            ((System.ComponentModel.ISupportInitialize)(this.newPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.oldPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AddPictureButton;
        private System.Windows.Forms.Button SendPictureButton;
        private System.Windows.Forms.Button CancelWorkButton;
        private System.Windows.Forms.GroupBox filterBox;
        private System.Windows.Forms.PictureBox newPictureBox;
        private System.Windows.Forms.PictureBox oldPictureBox;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label percent;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
    }
}

