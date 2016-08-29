namespace WinForm
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
            this.GraphListCmb = new System.Windows.Forms.ComboBox();
            this.GrapBoxPctb = new System.Windows.Forms.PictureBox();
            this.GoBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ScaleBar = new System.Windows.Forms.HScrollBar();
            ((System.ComponentModel.ISupportInitialize)(this.GrapBoxPctb)).BeginInit();
            this.SuspendLayout();
            // 
            // GraphListCmb
            // 
            this.GraphListCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GraphListCmb.FormattingEnabled = true;
            this.GraphListCmb.Items.AddRange(new object[] {
            "Ellips",
            "Parabola"});
            this.GraphListCmb.Location = new System.Drawing.Point(12, 12);
            this.GraphListCmb.Name = "GraphListCmb";
            this.GraphListCmb.Size = new System.Drawing.Size(121, 21);
            this.GraphListCmb.TabIndex = 0;
            // 
            // GrapBoxPctb
            // 
            this.GrapBoxPctb.Location = new System.Drawing.Point(12, 39);
            this.GrapBoxPctb.Name = "GrapBoxPctb";
            this.GrapBoxPctb.Size = new System.Drawing.Size(508, 241);
            this.GrapBoxPctb.TabIndex = 1;
            this.GrapBoxPctb.TabStop = false;
            // 
            // GoBtn
            // 
            this.GoBtn.Location = new System.Drawing.Point(139, 10);
            this.GoBtn.Name = "GoBtn";
            this.GoBtn.Size = new System.Drawing.Size(75, 23);
            this.GoBtn.TabIndex = 2;
            this.GoBtn.Text = "Print";
            this.GoBtn.UseVisualStyleBackColor = true;
            this.GoBtn.Click += new System.EventHandler(this.GoBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(341, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Scale:";
            this.ScaleBar.Value = 50;
            this.ScaleBar.ValueChanged += new System.EventHandler(this.ScaleBar_ValueChanged);
            // 
            // ScaleBar
            // 
            this.ScaleBar.Location = new System.Drawing.Point(391, 10);
            this.ScaleBar.Maximum = 200;
            this.ScaleBar.Minimum = 10;
            this.ScaleBar.Name = "ScaleBar";
            this.ScaleBar.Size = new System.Drawing.Size(124, 18);
            this.ScaleBar.TabIndex = 4;
            this.ScaleBar.Value = 100;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 287);
            this.Controls.Add(this.ScaleBar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GoBtn);
            this.Controls.Add(this.GrapBoxPctb);
            this.Controls.Add(this.GraphListCmb);
            this.Name = "Form1";
            this.Text = "Graphics";
            ((System.ComponentModel.ISupportInitialize)(this.GrapBoxPctb)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox GraphListCmb;
        private System.Windows.Forms.PictureBox GrapBoxPctb;
        private System.Windows.Forms.Button GoBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.HScrollBar ScaleBar;
    }
}

