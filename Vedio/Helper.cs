using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Vedio
{
    public class Helper
    {
        /// <summary>
        /// Trans Bitmap To BitmapSource
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")] public static extern bool DeleteObject(IntPtr hObject);
        public static BitmapSource BitmapToBitmapSource(Bitmap bitmap)
        {
            IntPtr ptr = bitmap.GetHbitmap();
            BitmapSource result =
                Imaging.CreateBitmapSourceFromHBitmap(ptr, 
                                                      IntPtr.Zero, 
                                                      Int32Rect.Empty, 
                                                      BitmapSizeOptions.FromEmptyOptions());
            //release resource
            DeleteObject(ptr);
            return result;
        }
    }
}
