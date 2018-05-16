using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vedio.Model
{
    public interface IVedioCaptureInfo
    {
        int Width { get; }
        int Height { get; }
        int CaptureFps { get; }
    }

    public class ScreenCaptureInfo : IVedioCaptureInfo
    {
        private Rectangle CurrentBounds => Screen.PrimaryScreen.Bounds;
        public int Width => CurrentBounds.Width;
        public int Height => CurrentBounds.Height;
        public int CaptureFps => 25;
    }
}
