namespace TestThreeOne
{
    partial class TestThreeOne
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
            this.ThirdTest = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.ThirdTest)).BeginInit();
            this.SuspendLayout();
            // 
            // ThirdTest
            // 
            chartArea1.Name = "ChartArea";
            this.ThirdTest.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend";
            this.ThirdTest.Legends.Add(legend1);
            this.ThirdTest.Location = new System.Drawing.Point(0, 0);
            this.ThirdTest.Margin = new System.Windows.Forms.Padding(7, 10, 7, 10);
            this.ThirdTest.Name = "ThirdTest";
            this.ThirdTest.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Berry;
            this.ThirdTest.Size = new System.Drawing.Size(600, 650);
            this.ThirdTest.TabIndex = 0;
            this.ThirdTest.Text = "First and Third Test";
            // 
            // TestThreeOne
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 43F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1155, 1597);
            this.Controls.Add(this.ThirdTest);
            this.Font = new System.Drawing.Font("Segoe Print", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(7, 10, 7, 10);
            this.Name = "TestThreeOne";
            this.Text = "First and Third Test";
            this.Load += new System.EventHandler(this.TestThreeOne_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ThirdTest)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart ThirdTest;
    }
}
