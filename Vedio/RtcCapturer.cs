using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRtc.NET;

namespace Vedio
{
    public class RtcCapturer : Capturer
    {
        private WebRtcNative _WebRtc;

        public RtcCapturer() : base() { }
        public RtcCapturer(WebRtcNative webRtc) : base()
        {
            _WebRtc = webRtc;
        }


        public void InjectRtc(WebRtcNative webRtc)
        {
            _WebRtc = webRtc;
            _WebRtc.SetCaptureWindows(true);
        }

        protected override void CaptureProcess()
        {
            if (_WebRtc == null) throw new Exception("WebRtcNative Must Be Injected");

            int x = -1, y = -1;
            ImgBufPtr = _WebRtc.CaptureFrameBGRX(ref x, ref y);
        }


    }
}
