using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vedio.Model;

namespace Vedio
{
    public class ScreenCapturer
    {
        private IVedioCaptureInfo _CaptureInfo = new ScreenCaptureInfo();


        byte[] ImgBuf;
        GCHandle BufHandle;
        public readonly IntPtr ImgBufPtr = IntPtr.Zero;
        Bitmap Img;
        Graphics Graphics;

        public ScreenCapturer()
        {
            ImgBuf= new byte[_CaptureInfo.Width * 3 * _CaptureInfo.Width];
            BufHandle = GCHandle.Alloc(ImgBuf, GCHandleType.Pinned);
            ImgBufPtr = BufHandle.AddrOfPinnedObject();
        }


        /// <summary>
        /// Capture Current View
        /// </summary>
        /// <returns></returns>
        public Bitmap CaptureView()
        {
            try
            {
                Img = new Bitmap(_CaptureInfo.Width, _CaptureInfo.Height, _CaptureInfo.Width * 3, PixelFormat.Format24bppRgb, ImgBufPtr);
                Graphics = Graphics.FromImage(Img);
                Graphics.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(_CaptureInfo.Width, _CaptureInfo.Height));
                return Img;
            }
            catch (Exception ex)
            {
                throw new Exception($"Capture Error:{ex.Message}");
            }
        }


    }
}
