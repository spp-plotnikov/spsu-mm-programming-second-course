﻿namespace Forms
{
    partial class FiltersApp
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
            this.ListOfFilters = new System.Windows.Forms.ComboBox();
            this.LoadButton = new System.Windows.Forms.Button();
            this.Source = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Target = new System.Windows.Forms.PictureBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.Source)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Target)).BeginInit();
            this.SuspendLayout();
            // 
            // ListOfFilters
            // 
            this.ListOfFilters.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(142)))), ((int)(((byte)(179)))));
            this.ListOfFilters.Cursor = System.Windows.Forms.Cursors.Default;
            this.ListOfFilters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ListOfFilters.Enabled = false;
            this.ListOfFilters.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ListOfFilters.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.ListOfFilters.FormattingEnabled = true;
            this.ListOfFilters.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ListOfFilters.Location = new System.Drawing.Point(197, 20);
            this.ListOfFilters.Name = "ListOfFilters";
            this.ListOfFilters.Size = new System.Drawing.Size(127, 24);
            this.ListOfFilters.Sorted = true;
            this.ListOfFilters.TabIndex = 0;
            this.ListOfFilters.SelectedIndexChanged += new System.EventHandler(this.ListOfFilters_SelectedIndexChanged);
            // 
            // LoadButton
            // 
            this.LoadButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(142)))), ((int)(((byte)(179)))));
            this.LoadButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LoadButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.LoadButton.Location = new System.Drawing.Point(8, 17);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(108, 27);
            this.LoadButton.TabIndex = 1;
            this.LoadButton.Text = "Load file";
            this.LoadButton.UseVisualStyleBackColor = false;
            this.LoadButton.Click += new System.EventHandler(this.Load_Click);
            // 
            // Source
            // 
            this.Source.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.Source.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Source.Location = new System.Drawing.Point(0, 11);
            this.Source.Name = "Source";
            this.Source.Size = new System.Drawing.Size(315, 258);
            this.Source.TabIndex = 2;
            this.Source.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.Source);
            this.panel2.Location = new System.Drawing.Point(8, 72);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(315, 279);
            this.panel2.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.Target);
            this.panel1.Location = new System.Drawing.Point(351, 72);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(315, 279);
            this.panel1.TabIndex = 4;
            // 
            // Target
            // 
            this.Target.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.Target.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Target.Location = new System.Drawing.Point(0, 11);
            this.Target.Name = "Target";
            this.Target.Size = new System.Drawing.Size(315, 258);
            this.Target.TabIndex = 3;
            this.Target.TabStop = false;
            // 
            // StartButton
            // 
            this.StartButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(142)))), ((int)(((byte)(179)))));
            this.StartButton.Enabled = false;
            this.StartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StartButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.StartButton.Location = new System.Drawing.Point(352, 20);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(107, 27);
            this.StartButton.TabIndex = 6;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = false;
            this.StartButton.Click += new System.EventHandler(this.Start_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // FiltersApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(678, 376);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.ListOfFilters);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(142)))), ((int)(((byte)(179)))));
            this.Name = "FiltersApp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Filters App";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            ((System.ComponentModel.ISupportInitialize)(this.Source)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Target)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox ListOfFilters;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.PictureBox Source;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox Target;
        private System.Windows.Forms.Button StartButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
    }
}

