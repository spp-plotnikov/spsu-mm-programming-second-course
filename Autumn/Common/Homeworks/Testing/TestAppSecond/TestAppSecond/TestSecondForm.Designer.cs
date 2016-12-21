namespace TestAppSecond
{
    partial class TestSecondForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.SecondTest = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.SecondTest)).BeginInit();
            this.SuspendLayout();
            // 
            // SecondTest
            // 
            chartArea1.Name = "ChartArea";
            this.SecondTest.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend";
            this.SecondTest.Legends.Add(legend1);
            this.SecondTest.Location = new System.Drawing.Point(0, 0);
            this.SecondTest.Margin = new System.Windows.Forms.Padding(7, 10, 7, 10);
            this.SecondTest.Name = "SecondTest";
            this.SecondTest.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Berry;
            this.SecondTest.Size = new System.Drawing.Size(461, 470);
            this.SecondTest.TabIndex = 0;
            this.SecondTest.Text = "SecondTest";
            // 
            // TestSecondForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 43F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1155, 1597);
            this.Controls.Add(this.SecondTest);
            this.Font = new System.Drawing.Font("Segoe Print", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(7, 10, 7, 10);
            this.Name = "TestSecondForm";
            this.Text = "SecondTest";
            this.Load += new System.EventHandler(this.TestSecondForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SecondTest)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart SecondTest;
    }
}
