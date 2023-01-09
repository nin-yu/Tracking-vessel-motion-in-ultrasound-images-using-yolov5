using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace yolo_csharp.Core
{
    public class DetChart
    {
        public void ChartInit(Chart chart)
        {
            #region 定义图表区域、设置图表显示样式
            chart.ChartAreas.Clear();
            ChartArea chartArea = new ChartArea("C1");

            //设置图表显示样式
            chartArea.AxisY.Minimum = 400.0; // Y轴最小值 
            chartArea.AxisY.Maximum = 200.0; // Y轴最大值
            chartArea.AxisY.IsStartedFromZero = false;
            chartArea.AxisY.MajorGrid.Enabled = true;

            // 背景样式
            chartArea.BackColor = Color.White;                       //背景色
            chartArea.BackSecondaryColor = Color.White;              //渐变背景色
            chartArea.BackGradientStyle = GradientStyle.TopBottom;   //渐变方式
            chartArea.BackHatchStyle = ChartHatchStyle.None;         //背景阴影
            chartArea.BorderDashStyle = ChartDashStyle.NotSet;       //边框线样式
            chartArea.BorderWidth = 1;                               //边框宽度
            chartArea.BorderColor = Color.Black;

            chart.ChartAreas.Add(chartArea);
            #endregion

            #region Series 数据初始化
            chart.Series.Clear();
            Series series = new Series("line1");
            series.ChartArea = "C1";
            series.Color = Color.Red;
            series.Points.Clear();
            series.XValueType = ChartValueType.String;
            series.YValueType = ChartValueType.Double;
            series.BorderWidth = 1;
            series.MarkerColor = Color.Green;
            series.MarkerSize = 7;
            series.MarkerStyle = MarkerStyle.None;  // MarkerStyle.Circle
            series.ChartType = SeriesChartType.Line; // Line折线图 Spline 曲线图
            series.IsValueShownAsLabel = false; // 是否在标签上显示数值
            series.ToolTip = "时间：#VALX\n当前值：#VALY\n最大值：#MAX\n最小值：#MIN\n平均值：#AVG";
            chart.Series.Add(series);

            #endregion

            #region 设置标题等样式
            chart.Titles.Clear();
            chart.Titles.Add("n1");
            //chart.Titles[0].Text = TabName;
            chart.Titles[0].ForeColor = Color.RoyalBlue;
            chart.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);

            // 设置边框
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BorderlineWidth = 1;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            #endregion
        }
    }

    public class ChartData
    {
        /// <summary>
        /// 曲线名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 曲线颜色
        /// </summary>
        public Color color { get; set; }
    }
}
