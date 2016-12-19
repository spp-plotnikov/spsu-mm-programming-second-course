namespace Client
{
    partial class FormFilters
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.LoadButton = new System.Windows.Forms.Button();
            this.FilterButton = new System.Windows.Forms.Button();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.FiltersList = new System.Windows.Forms.ComboBox();
            this.FileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.CancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // PictureBox
            // 
            this.PictureBox.Location = new System.Drawing.Point(51, 26);
            this.PictureBox.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(1307, 766);
            this.PictureBox.TabIndex = 0;
            this.PictureBox.TabStop = false;
            // 
            // LoadButton
            // 
            this.LoadButton.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LoadButton.Location = new System.Drawing.Point(51, 931);
            this.LoadButton.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(476, 128);
            this.LoadButton.TabIndex = 1;
            this.LoadButton.Text = "Загрузить изображение";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // FilterButton
            // 
            this.FilterButton.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FilterButton.Location = new System.Drawing.Point(1400, 919);
            this.FilterButton.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.FilterButton.Name = "FilterButton";
            this.FilterButton.Size = new System.Drawing.Size(476, 138);
            this.FilterButton.TabIndex = 1;
            this.FilterButton.Text = "Применить фильтр";
            this.FilterButton.UseVisualStyleBackColor = true;
            this.FilterButton.Click += new System.EventHandler(this.FilterButton_Click);
            // 
            // ProgressBar
            // 
            this.ProgressBar.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ProgressBar.Location = new System.Drawing.Point(541, 954);
            this.ProgressBar.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(845, 70);
            this.ProgressBar.TabIndex = 2;
            // 
            // FiltersList
            // 
            this.FiltersList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FiltersList.Location = new System.Drawing.Point(1475, 74);
            this.FiltersList.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.FiltersList.Name = "FiltersList";
            this.FiltersList.Size = new System.Drawing.Size(429, 37);
            this.FiltersList.TabIndex = 3;
            // 
            // FileDialog
            // 
            this.FileDialog.FileName = "openFileDialog1";
            // 
            // CancelButton
            // 
            this.CancelButton.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CancelButton.Location = new System.Drawing.Point(1400, 784);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(476, 123);
            this.CancelButton.TabIndex = 5;
            this.CancelButton.Text = "Отмена";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // FormFilters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1941, 1143);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.FiltersList);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.FilterButton);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.PictureBox);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Name = "FormFilters";
            this.Text = "Фильтры";
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PictureBox;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.ComboBox FiltersList;
        private System.Windows.Forms.OpenFileDialog FileDialog;
        private System.Windows.Forms.SaveFileDialog saveFile;
        public System.Windows.Forms.Button FilterButton;
        private System.Windows.Forms.Button CancelButton;
    }
}

