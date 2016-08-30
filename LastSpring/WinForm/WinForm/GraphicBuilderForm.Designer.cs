namespace WinForm
{
    partial class GraphicBuilderForm
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
            this.PrintBtn = new System.Windows.Forms.Button();
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
            "Ellipse",
            "Parabola"});
            this.GraphListCmb.Location = new System.Drawing.Point(12, 12);
            this.GraphListCmb.Name = "GraphListCmb";
            this.GraphListCmb.Size = new System.Drawing.Size(121, 21);
            this.GraphListCmb.TabIndex = 0;
            // 
            // GrapBoxPctb
            // 
            this.GrapBoxPctb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GrapBoxPctb.Location = new System.Drawing.Point(2, 1);
            this.GrapBoxPctb.Name = "GrapBoxPctb";
            this.GrapBoxPctb.Size = new System.Drawing.Size(528, 286);
            this.GrapBoxPctb.TabIndex = 1;
            this.GrapBoxPctb.TabStop = false;
            // 
            // PrintBtn
            // 
            this.PrintBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PrintBtn.Location = new System.Drawing.Point(445, 12);
            this.PrintBtn.Name = "PrintBtn";
            this.PrintBtn.Size = new System.Drawing.Size(75, 23);
            this.PrintBtn.TabIndex = 2;
            this.PrintBtn.Text = "Print";
            this.PrintBtn.UseVisualStyleBackColor = true;
            this.PrintBtn.Click += new System.EventHandler(this.PrintBtn_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(-1, 269);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Scale:";
            // 
            // ScaleBar
            // 
            this.ScaleBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ScaleBar.Location = new System.Drawing.Point(59, 269);
            this.ScaleBar.Maximum = 200;
            this.ScaleBar.Minimum = 10;
            this.ScaleBar.Name = "ScaleBar";
            this.ScaleBar.Size = new System.Drawing.Size(124, 18);
            this.ScaleBar.TabIndex = 4;
            this.ScaleBar.Value = 100;
            this.ScaleBar.ValueChanged += new System.EventHandler(this.ScaleBar_ValueChanged);
            // 
            // GraphicBuilderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 287);
            this.Controls.Add(this.ScaleBar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PrintBtn);
            this.Controls.Add(this.GraphListCmb);
            this.Controls.Add(this.GrapBoxPctb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "GraphicBuilderForm";
            this.Text = "Graphics";
            ((System.ComponentModel.ISupportInitialize)(this.GrapBoxPctb)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox GraphListCmb;
        private System.Windows.Forms.PictureBox GrapBoxPctb;
        private System.Windows.Forms.Button PrintBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.HScrollBar ScaleBar;
    }
}

