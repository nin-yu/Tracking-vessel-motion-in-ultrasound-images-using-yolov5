using System;
using System.Runtime.InteropServices;

namespace yolo_csharp.Core
{
    public class DllLinker
    {
        [DllImport(@"C:\\Users\\nin\\source\\repos\\dll_yolov5\\x64\\Release\\dll_yolov5.dll", EntryPoint = "test_mat", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr test_mat(byte[] img_src, int imageWedth, int imageHeight);

        [DllImport(@"C:\\Users\\nin\\source\\repos\\dll_yolov5\\x64\\Release\\dll_yolov5.dll", EntryPoint = "opencv_test_img", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr opencv_test_img(byte[] img_src, string labelTXT_path, int imageWedth, int imageHeight);

        [DllImport(@"C:\\Users\\nin\\source\\repos\\dll_yolov5\\x64\\Release\\dll_yolov5.dll", EntryPoint = "Init_yolo", CallingConvention = CallingConvention.Cdecl)]
        public extern static void yolo_init(string model_path);
    }
}