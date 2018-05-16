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
    public class ScreenCapturer : Capturer
    {
        protected Graphics Graphics;

        public ScreenCapturer()
            :base()
        {
            Graphics = Graphics.FromImage(Img);
        }

        protected override void CaptureProcess()
        {
            Graphics.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(_CaptureInfo.Width, _CaptureInfo.Height));
        }
    }
}
