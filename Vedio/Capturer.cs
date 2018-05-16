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
    public interface ICapturer
    {
        Bitmap CaptureView();
    }

    public abstract class Capturer : ICapturer
    {
        protected static object locker = new { };

        protected byte[] ImgBuf;
        protected IVedioCaptureInfo _CaptureInfo = new ScreenCaptureInfo();
        protected Bitmap Img;
        protected GCHandle BufHandle;

        public  Capturer()
        {
            ImgBuf = new byte[_CaptureInfo.Width * 4 * _CaptureInfo.Height];
            BufHandle = GCHandle.Alloc(ImgBuf, GCHandleType.Pinned);
            ImgBufPtr = BufHandle.AddrOfPinnedObject();
            Img = new Bitmap(_CaptureInfo.Width, _CaptureInfo.Height, _CaptureInfo.Width * 4, PixelFormat.Format32bppArgb, ImgBufPtr);
        }

        public IntPtr ImgBufPtr { get; protected set; }

        /// <summary>
        /// Capture Current View
        /// </summary>
        /// <returns></returns>
        public Bitmap CaptureView()
        {
            try
            {
                lock (locker)
                {
                    CaptureProcess();
                    return Img;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Capture Error:{ex.Message}");
            }
        }

        /// <summary>
        /// Capture Desktop Process, must to I/O Img
        /// </summary>
        protected abstract void CaptureProcess();
    }
}
