namespace Client
{
    partial class MyClient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyClient));
            this.chooseButton = new System.Windows.Forms.Button();
            this.cancelProcessingButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.imageProcessingButton = new System.Windows.Forms.Button();
            this.filterSelectionBox = new System.Windows.Forms.ComboBox();
            this.imageBox = new System.Windows.Forms.PictureBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // chooseButton
            // 
            this.chooseButton.BackColor = System.Drawing.Color.Pink;
            this.chooseButton.FlatAppearance.BorderSize = 0;
            this.chooseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chooseButton.Font = new System.Drawing.Font("Segoe Print", 10F);
            this.chooseButton.Location = new System.Drawing.Point(534, 26);
            this.chooseButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chooseButton.Name = "chooseButton";
            this.chooseButton.Size = new System.Drawing.Size(147, 47);
            this.chooseButton.TabIndex = 0;
            this.chooseButton.Text = "Choose image";
            this.chooseButton.UseVisualStyleBackColor = false;
            this.chooseButton.Click += new System.EventHandler(this.ChooseImage);
            // 
            // cancelProcessingButton
            // 
            this.cancelProcessingButton.BackColor = System.Drawing.Color.Pink;
            this.cancelProcessingButton.FlatAppearance.BorderSize = 0;
            this.cancelProcessingButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelProcessingButton.Location = new System.Drawing.Point(626, 205);
            this.cancelProcessingButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cancelProcessingButton.Name = "cancelProcessingButton";
            this.cancelProcessingButton.Size = new System.Drawing.Size(90, 56);
            this.cancelProcessingButton.TabIndex = 1;
            this.cancelProcessingButton.Text = "Cancel process";
            this.cancelProcessingButton.UseVisualStyleBackColor = false;
            this.cancelProcessingButton.Click += new System.EventHandler(this.CancelProcessingButtonClick);
            // 
            // saveButton
            // 
            this.saveButton.BackColor = System.Drawing.Color.Pink;
            this.saveButton.FlatAppearance.BorderSize = 0;
            this.saveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveButton.Font = new System.Drawing.Font("Segoe Print", 10F);
            this.saveButton.Location = new System.Drawing.Point(534, 321);
            this.saveButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(147, 46);
            this.saveButton.TabIndex = 2;
            this.saveButton.Text = "Save image";
            this.saveButton.UseVisualStyleBackColor = false;
            this.saveButton.Click += new System.EventHandler(this.SaveButtonClick);
            // 
            // imageProcessingButton
            // 
            this.imageProcessingButton.BackColor = System.Drawing.Color.Pink;
            this.imageProcessingButton.FlatAppearance.BorderSize = 0;
            this.imageProcessingButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.imageProcessingButton.Location = new System.Drawing.Point(502, 205);
            this.imageProcessingButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.imageProcessingButton.Name = "imageProcessingButton";
            this.imageProcessingButton.Size = new System.Drawing.Size(89, 56);
            this.imageProcessingButton.TabIndex = 3;
            this.imageProcessingButton.Text = "Apply filter";
            this.imageProcessingButton.UseVisualStyleBackColor = false;
            this.imageProcessingButton.Click += new System.EventHandler(this.SendImageToServer);
            // 
            // filterSelectionBox
            // 
            this.filterSelectionBox.BackColor = System.Drawing.Color.Snow;
            this.filterSelectionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterSelectionBox.Font = new System.Drawing.Font("Segoe Print", 9F);
            this.filterSelectionBox.ForeColor = System.Drawing.Color.DarkRed;
            this.filterSelectionBox.Location = new System.Drawing.Point(534, 129);
            this.filterSelectionBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.filterSelectionBox.Name = "filterSelectionBox";
            this.filterSelectionBox.Size = new System.Drawing.Size(147, 34);
            this.filterSelectionBox.TabIndex = 4;
            this.filterSelectionBox.TabStop = false;
            this.filterSelectionBox.SelectedIndexChanged += new System.EventHandler(this.FilterSelected);
            // 
            // imageBox
            // 
            this.imageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox.ErrorImage = null;
            this.imageBox.Location = new System.Drawing.Point(28, 26);
            this.imageBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.imageBox.Name = "imageBox";
            this.imageBox.Size = new System.Drawing.Size(429, 341);
            this.imageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox.TabIndex = 5;
            this.imageBox.TabStop = false;
            // 
            // progressBar
            // 
            this.progressBar.BackColor = System.Drawing.Color.Pink;
            this.progressBar.ForeColor = System.Drawing.Color.DarkRed;
            this.progressBar.Location = new System.Drawing.Point(28, 408);
            this.progressBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(688, 33);
            this.progressBar.Step = 3;
            this.progressBar.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe Print", 10F);
            this.label1.Location = new System.Drawing.Point(560, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 30);
            this.label1.TabIndex = 8;
            this.label1.Text = "Choose filter";
            // 
            // MyClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.LavenderBlush;
            this.ClientSize = new System.Drawing.Size(744, 464);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.imageBox);
            this.Controls.Add(this.filterSelectionBox);
            this.Controls.Add(this.imageProcessingButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.cancelProcessingButton);
            this.Controls.Add(this.chooseButton);
            this.Font = new System.Drawing.Font("Segoe Print", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.Color.DarkRed;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "MyClient";
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button chooseButton;
        private System.Windows.Forms.Button cancelProcessingButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button imageProcessingButton;
        private System.Windows.Forms.ComboBox filterSelectionBox;
        private System.Windows.Forms.PictureBox imageBox;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label1;
    }
}

