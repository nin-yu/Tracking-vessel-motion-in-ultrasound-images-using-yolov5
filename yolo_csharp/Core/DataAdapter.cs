using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace yolo_csharp.Core
{
    public class DataAdapter
    {
        public static List<RectModel> DetRects(Mat img)
        {
            string laeblTXT_path = "C:/Users/nin/source/repos/test_libtorch/test_libtorch/coco_vessel.txt";

            //扫描宽度
            int s_width;
            //扫描高度
            int s_height;
            //图像实际宽度
            int imgWidth;
            //图像实际高度
            int imgHeight;

            imgWidth = img.Width;
            imgHeight = img.Height;

            //将图像数据lock在内存
            Bitmap bmp = img.ToBitmap();
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            //------------判断实际应该显示的大小--------------
            s_width = bmpData.Stride;
            s_height = bmp.Height;
            imgHeight = bmpData.Height;
            if (s_width > imgWidth * 3)
            {
                imgWidth++;
            }
            //------------------------------------------------
            int bytes = s_width * s_height;
            byte[] rgbValues = new byte[bytes];
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            IntPtr repI = DllLinker.opencv_test_img(rgbValues, laeblTXT_path, imgWidth, imgHeight);  // 检测图片
            if (repI == IntPtr.Zero)
            {
                return null;
            }
            string strContent = Marshal.PtrToStringAnsi(repI);
            List<RectModel> result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RectModel>>(strContent,
                         new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });

            //相关释放
            bmp.UnlockBits(bmpData);
            //Marshal.FreeHGlobal(repI); 

            return result;
        }
        public static void InitYolo()
        {
            string model_path = "C:/Users/nin/source/repos/test_libtorch/test_libtorch/yolov5_vessel.pt";
            DllLinker.yolo_init(model_path);    // 加载模型
            
        }
    }
}