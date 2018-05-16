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
    public class WindowCapture
    {
        protected Graphics Graphics;
        protected static object locker = new { };
        protected byte[] ImgBuf;
        protected Bitmap Img;
        protected GCHandle BufHandle;
        public IntPtr ImgBufPtr { get; private set; }
        //public IntPtr F_Ptr = IntPtr.Zero;

        /// <summary>
        /// Capture Current View
        /// </summary>
        /// <returns></returns>
        public Bitmap CaptureView(IntPtr hWnd)
        {
            try
            {

                lock (locker)
                {
                    IntPtr fPtr = WindowHelper.Helper.GetForegroundWindow();
                    Rectangle windowRtg = WindowHelper.Helper.GetWindowCoordinate(hWnd);
                    ResetContent(hWnd, windowRtg.Width, windowRtg.Height);
                    //Graphics.CopyFromScreen(new Point(windowRtg.X, windowRtg.Y), new Point(0, 0), new Size(windowRtg.Width, windowRtg.Height));

                    if (hWnd == fPtr)
                        Graphics.CopyFromScreen(new Point(windowRtg.X, windowRtg.Y), new Point(0, 0), new Size(windowRtg.Width, windowRtg.Height));
                    //else
                    //    Graphics.FillRectangle(new SolidBrush(Color.Black), windowRtg);
                    return Img;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Capture Error:{ex.Message}");

            }
        }

        private int oWidth { get; set; }
        private int oHeight { get; set; }

        private void ResetContent(IntPtr hWnd, int width, int height)
        {
            if (oWidth == width && oHeight == height) return;

            oWidth = width;
            oHeight = height;

            ImgBuf = new byte[width * 4 * height];
            BufHandle = GCHandle.Alloc(ImgBuf, GCHandleType.Pinned);
            ImgBufPtr = BufHandle.AddrOfPinnedObject();
            Img = new Bitmap(width, height, width * 4, PixelFormat.Format32bppArgb, ImgBufPtr);
            Graphics = Graphics.FromImage(Img);
        }
       
    }
}
