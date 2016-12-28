namespace TimeVsImage
{
    partial class MyTimeVsImage
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
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyTimeVsImage));
            this.timeVsImageChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.timeVsImageChart)).BeginInit();
            this.SuspendLayout();
            // 
            // timeVsImageChart
            // 
            this.timeVsImageChart.BackColor = System.Drawing.Color.LavenderBlush;
            this.timeVsImageChart.BorderlineWidth = 0;
            this.timeVsImageChart.BorderSkin.BorderWidth = 0;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisX.Title = "Image size";
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Segoe Print", 9F);
            chartArea1.AxisY.Title = "Time";
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Segoe Print", 9F);
            chartArea1.BackColor = System.Drawing.Color.LavenderBlush;
            chartArea1.BorderColor = System.Drawing.Color.DarkRed;
            chartArea1.Name = "ChartArea";
            this.timeVsImageChart.ChartAreas.Add(chartArea1);
            legend1.BackColor = System.Drawing.Color.LavenderBlush;
            legend1.Font = new System.Drawing.Font("Segoe Print", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            legend1.IsTextAutoFit = false;
            legend1.Name = "Legend1";
            this.timeVsImageChart.Legends.Add(legend1);
            this.timeVsImageChart.Location = new System.Drawing.Point(12, 12);
            this.timeVsImageChart.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.timeVsImageChart.Name = "timeVsImageChart";
            this.timeVsImageChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Light;
            series1.BackImageWrapMode = System.Windows.Forms.DataVisualization.Charting.ChartImageWrapMode.Scaled;
            series1.ChartArea = "ChartArea";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.DeepPink;
            series1.Legend = "Legend1";
            series1.MarkerStep = 1000;
            series1.Name = "Average";
            series2.BackImageWrapMode = System.Windows.Forms.DataVisualization.Charting.ChartImageWrapMode.Scaled;
            series2.BorderColor = System.Drawing.Color.White;
            series2.ChartArea = "ChartArea";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.Lime;
            series2.Legend = "Legend1";
            series2.MarkerStep = 1000;
            series2.Name = "Median";
            this.timeVsImageChart.Series.Add(series1);
            this.timeVsImageChart.Series.Add(series2);
            this.timeVsImageChart.Size = new System.Drawing.Size(558, 577);
            this.timeVsImageChart.TabIndex = 0;
            // 
            // MyTimeVsImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.LavenderBlush;
            this.ClientSize = new System.Drawing.Size(582, 603);
            this.Controls.Add(this.timeVsImageChart);
            this.Font = new System.Drawing.Font("Segoe Print", 8F);
            this.ForeColor = System.Drawing.Color.DarkRed;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.MaximizeBox = false;
            this.Name = "MyTimeVsImage";
            this.Text = "TimeVsImage";
            ((System.ComponentModel.ISupportInitialize)(this.timeVsImageChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart timeVsImageChart;
    }
}

