namespace TimeVsClients
{
    partial class MyTimeVsClients
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
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyTimeVsClients));
            this.timeVsClientsChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.maxClientsLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.timeVsClientsChart)).BeginInit();
            this.SuspendLayout();
            // 
            // timeVsClientsChart
            // 
            this.timeVsClientsChart.BackColor = System.Drawing.Color.LavenderBlush;
            this.timeVsClientsChart.BorderlineWidth = 0;
            this.timeVsClientsChart.BorderSkin.BorderWidth = 0;
            chartArea1.AxisX.Title = "Number of clients";
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Segoe Print", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            chartArea1.AxisY.Title = "Time";
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Segoe Print", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            chartArea1.BackColor = System.Drawing.Color.LavenderBlush;
            chartArea1.Name = "ChartArea";
            this.timeVsClientsChart.ChartAreas.Add(chartArea1);
            this.timeVsClientsChart.Location = new System.Drawing.Point(12, 65);
            this.timeVsClientsChart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.timeVsClientsChart.Name = "timeVsClientsChart";
            this.timeVsClientsChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Light;
            series1.ChartArea = "ChartArea";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.DarkRed;
            series1.Name = "Series";
            this.timeVsClientsChart.Series.Add(series1);
            this.timeVsClientsChart.Size = new System.Drawing.Size(558, 524);
            this.timeVsClientsChart.TabIndex = 0;
            // 
            // maxClientsLabel
            // 
            this.maxClientsLabel.AutoSize = true;
            this.maxClientsLabel.Font = new System.Drawing.Font("Segoe Print", 10F);
            this.maxClientsLabel.Location = new System.Drawing.Point(153, 24);
            this.maxClientsLabel.Name = "maxClientsLabel";
            this.maxClientsLabel.Size = new System.Drawing.Size(0, 30);
            this.maxClientsLabel.TabIndex = 1;
            // 
            // MyTimeVsClients
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.LavenderBlush;
            this.ClientSize = new System.Drawing.Size(582, 603);
            this.Controls.Add(this.maxClientsLabel);
            this.Controls.Add(this.timeVsClientsChart);
            this.Font = new System.Drawing.Font("Segoe Print", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.Color.DarkRed;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "MyTimeVsClients";
            this.Text = "TimeVsClients";
            ((System.ComponentModel.ISupportInitialize)(this.timeVsClientsChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart timeVsClientsChart;
        private System.Windows.Forms.Label maxClientsLabel;
    }
}

