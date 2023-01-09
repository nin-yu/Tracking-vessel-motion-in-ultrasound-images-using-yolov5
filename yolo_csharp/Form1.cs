using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using OpenCvSharp.Extensions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using OpenCvSharp;
using System.Threading;
using yolo_csharp.Core;
using System.Diagnostics;

namespace yolo_csharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //摄像机播放线程
        Thread videoThread;
        VideoCapture objCap;

        /// <summary>
        ///  横坐标最初值
        /// </summary>
        private DateTime X_minValue;

        /// <summary>
        /// 随机数
        /// </summary>
        private Random rand = new Random();

        /// <summary>
        /// 曲线个数，最大值6
        /// </summary>
        private int LineNum { get; set; } = 6;
        /// <summary>
        /// 不停的添加数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                // 添加数据
                this.chart1.Series[0].Points.AddXY(DateTime.Now.ToOADate(), rand.Next(5, 20));

                // X坐标后移1秒
                this.chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.AddSeconds(1).ToOADate();

                if (checkBox1.Checked == true)
                    chart1.ChartAreas[0].AxisX.Minimum = DateTime.Now.AddSeconds(-10).ToOADate();//此刻后10分钟作为最初X轴，
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        #region 初始化检测模型，检测图片和视频
        // 初始化模型，加载模型文件
        private void InitYolo_Click(object sender, EventArgs e)
        {
            DataAdapter.InitYolo();
            //testVideo();
        }
        // 检测单独一张图片
        private void DetImg_Click(object sender, EventArgs e)
        {
            Mat img = new Mat(@"C:/Users/nin/source/repos/yolov5_csharp/1.jpg", ImreadModes.Color);
            List<RectModel> rects = DataAdapter.DetRects(img);  // 检测图像，返回框的坐标
            foreach (RectModel item in rects)
            {
                img.Rectangle(
                    new OpenCvSharp.Point(item.Left, item.Top), //左上
                    new OpenCvSharp.Point(item.Right, item.Bottom),  //右下
                    Scalar.Red,  //线色
                    2  //线粗
                    );
            }   // 绘制检测框
            Cv2.ImShow("1", img);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }
        // 视频流检测
        private void DetVideo_Click(object sender, EventArgs e)
        {
            string inputVideoPath = "D:/dataset/jiale/01_6.mp4";
            VideoCapture capture = new VideoCapture(inputVideoPath);

            while (true)
            {
                Mat frame = new Mat();
                if (capture.Read(frame))
                {
                    // 检测一帧的图像并计算耗时
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    List<RectModel> rects = DataAdapter.DetRects(frame);
                    watch.Stop();
                    string Text = $"{watch.ElapsedMilliseconds} ms." + $" FPS: {1000f / watch.ElapsedMilliseconds}";
                    // string Text = $"Frame Processing time: {watch.ElapsedMilliseconds} ms." + $" FPS: {1000f / watch.ElapsedMilliseconds}";

                    if (rects != null && rects.Count > 0)
                    {
                        // 绘制血管框选区
                        int i = 0;
                        foreach (RectModel item in rects)
                        {
                            frame.Rectangle(
                                new OpenCvSharp.Point(item.Left, item.Top), //左上
                                new OpenCvSharp.Point(item.Right, item.Bottom),  //右下
                                Scalar.Red,  //线色n
                                2  //线粗
                                );
                            this.chart1.Series[i].Points.AddXY(i, item.Bottom);
                            i++;    
                        }
                        Cv2.PutText(frame, $"{watch.ElapsedMilliseconds} ms."+ $" FPS: {1000f / watch.ElapsedMilliseconds}", new OpenCvSharp.Point(50, 60), HersheyFonts.HersheyComplex, 1.01, Scalar.White);
                    }
                    //Image imgshow = frame.ToBitmap();
                    //pictureBox1.Image = imgshow;
                    //imgshow.Dispose();
                    Cv2.ImShow("Track", frame);
                    Cv2.WaitKey(1);
                }
                else { break; }
                //释放
                frame.Release();
                GC.Collect(); //这里要使用gc,手动释放会造成GDI异常
            }
            Cv2.DestroyAllWindows();
            capture.Release();
        }
        // 摄像头检测
        private void testCamera_Click(object sender, EventArgs e)
        {
            VideoCapture capture = new VideoCapture(0);
            if (capture.IsOpened())
            {
                Console.WriteLine("摄像头已经打开");
            }
            else
            {
                MessageBox.Show("警告，摄像头未开启！");
                return;
            }
            //图像人脸识别(注意,这里是true死循环,跳出循视频请Abort)
            while (true)
            {
                //读取一帧
                //Mat myImage = new Mat(cs_pic,ImreadModes.Color);//测试
                Mat myImage = new Mat();
                capture.Read(myImage);

                //Cv2.Flip(myImage, myImage, FlipMode.XY);

                if (myImage.Empty()) //判断当前帧是否捕捉成功 这步很重要
                {
                    Cv2.WaitKey(60);
                    myImage.Release();
                    continue;
                }
                Cv2.ImShow("摄像头图像", myImage); //测试
                Image imgshow = myImage.ToBitmap();

                myImage.Release();//手动释放
                //imgshow.Dispose(); 
                GC.Collect(); //这里要使用gc,手动释放会造成GDI异常
                int keyValue = Cv2.WaitKey(10);
                if (keyValue == 113 || keyValue == 81)
                {
                    capture.Release();
                    break;
                }
            }
        }
        #endregion

        #region 图表样式控制
        private void chart1_Click(object sender, EventArgs e)
        {

        }
        // 初始化图表
        private void button1_Click(object sender, EventArgs e)
        {
            new DetChart().ChartInit(chart1);
            chart1.ChartAreas[0].AxisY.Maximum = 410;
            chart1.ChartAreas[0].AxisY.Minimum = 390;
        }
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)           //X轴设置成显示最初时间坐标
                chart1.ChartAreas[0].AxisX.Minimum = X_minValue.ToOADate();       //最初打开时候为X轴
        }

        private void button4_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.Minimum = DateTime.Now.AddSeconds(-3).ToOADate();//此刻后三秒
        }

        private void button5_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.Minimum = X_minValue.ToOADate();
        }
        #endregion

        
    }
}
