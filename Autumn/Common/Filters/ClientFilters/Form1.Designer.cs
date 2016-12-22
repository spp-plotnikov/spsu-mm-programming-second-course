namespace ClientFilters
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BeforeImageBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.filterNameComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.openImageButton = new System.Windows.Forms.Button();
            this.sendImageButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.fileName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.AfterImageBox = new System.Windows.Forms.PictureBox();
            this.updateFilterListButton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.sendSetFilterNReceiveWorker = new System.ComponentModel.BackgroundWorker();
            this.checkingProgressWorker = new System.ComponentModel.BackgroundWorker();
            this.label6 = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.cancellationWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.BeforeImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AfterImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // BeforeImageBox
            // 
            this.BeforeImageBox.Location = new System.Drawing.Point(12, 12);
            this.BeforeImageBox.Name = "BeforeImageBox";
            this.BeforeImageBox.Size = new System.Drawing.Size(262, 216);
            this.BeforeImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.BeforeImageBox.TabIndex = 0;
            this.BeforeImageBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(124, 231);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Before";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(430, 231);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "After";
            // 
            // filterNameComboBox
            // 
            this.filterNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterNameComboBox.FormattingEnabled = true;
            this.filterNameComboBox.Location = new System.Drawing.Point(79, 283);
            this.filterNameComboBox.Name = "filterNameComboBox";
            this.filterNameComboBox.Size = new System.Drawing.Size(411, 21);
            this.filterNameComboBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 286);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Filter:";
            // 
            // openImageButton
            // 
            this.openImageButton.Location = new System.Drawing.Point(496, 247);
            this.openImageButton.Name = "openImageButton";
            this.openImageButton.Size = new System.Drawing.Size(79, 26);
            this.openImageButton.TabIndex = 7;
            this.openImageButton.Text = "Open image";
            this.openImageButton.UseVisualStyleBackColor = true;
            this.openImageButton.Click += new System.EventHandler(this.openImageButton_Click);
            // 
            // sendImageButton
            // 
            this.sendImageButton.Location = new System.Drawing.Point(496, 311);
            this.sendImageButton.Name = "sendImageButton";
            this.sendImageButton.Size = new System.Drawing.Size(79, 26);
            this.sendImageButton.TabIndex = 8;
            this.sendImageButton.Text = "Send image";
            this.sendImageButton.UseVisualStyleBackColor = true;
            this.sendImageButton.Click += new System.EventHandler(this.sendImageButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // fileName
            // 
            this.fileName.Location = new System.Drawing.Point(79, 251);
            this.fileName.Name = "fileName";
            this.fileName.ReadOnly = true;
            this.fileName.Size = new System.Drawing.Size(411, 20);
            this.fileName.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 254);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Name of file:";
            // 
            // AfterImageBox
            // 
            this.AfterImageBox.Location = new System.Drawing.Point(313, 12);
            this.AfterImageBox.Name = "AfterImageBox";
            this.AfterImageBox.Size = new System.Drawing.Size(262, 216);
            this.AfterImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.AfterImageBox.TabIndex = 11;
            this.AfterImageBox.TabStop = false;
            // 
            // updateFilterListButton
            // 
            this.updateFilterListButton.Location = new System.Drawing.Point(496, 279);
            this.updateFilterListButton.Name = "updateFilterListButton";
            this.updateFilterListButton.Size = new System.Drawing.Size(79, 26);
            this.updateFilterListButton.TabIndex = 12;
            this.updateFilterListButton.Text = "Update list";
            this.updateFilterListButton.UseVisualStyleBackColor = true;
            this.updateFilterListButton.Click += new System.EventHandler(this.updateFilterListButton_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(79, 313);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(411, 20);
            this.progressBar.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 318);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Progress:";
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(79, 339);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(411, 31);
            this.cancelButton.TabIndex = 15;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.button4_Click);
            // 
            // sendSetFilterNReceiveWorker
            // 
            this.sendSetFilterNReceiveWorker.WorkerSupportsCancellation = true;
            this.sendSetFilterNReceiveWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.SendNGetImage);
            // 
            // checkingProgressWorker
            // 
            this.checkingProgressWorker.WorkerSupportsCancellation = true;
            this.checkingProgressWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.UpdatingProgressBar);
            //
            // cancellationWorker 
            //
            this.cancellationWorker.WorkerSupportsCancellation = true;
            this.cancellationWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Cancellation);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(496, 348);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Status:";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(535, 348);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 13);
            this.statusLabel.TabIndex = 17;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 382);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.updateFilterListButton);
            this.Controls.Add(this.AfterImageBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.fileName);
            this.Controls.Add(this.sendImageButton);
            this.Controls.Add(this.openImageButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.filterNameComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BeforeImageBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.BeforeImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AfterImageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox BeforeImageBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button openImageButton;
        private System.Windows.Forms.Button sendImageButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox fileName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox AfterImageBox;
        private System.Windows.Forms.Button updateFilterListButton;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button cancelButton;
        private System.ComponentModel.BackgroundWorker sendSetFilterNReceiveWorker;
        private System.ComponentModel.BackgroundWorker checkingProgressWorker;
        protected System.Windows.Forms.ComboBox filterNameComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label statusLabel;
        private System.ComponentModel.BackgroundWorker cancellationWorker;
    }
}

