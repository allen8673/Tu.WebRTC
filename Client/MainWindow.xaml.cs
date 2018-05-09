using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tu.WebRTC.RctPoint;
using Vedio.Model;
using System.Drawing;
using Vedio;
using System.Diagnostics;

namespace Client
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        RtcClient _ClientPeer;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_ClientPeer == null)
            {
                _ClientPeer = new RtcClient(SocketUrl.Text);
                // Set RenderLocal callback
                _ClientPeer.SetRenderLocal((BGR24, w, h) =>
                {
                    RefreshLocalView();
                });
                // Set RenderRemote callback
                _ClientPeer.SetRenderRemote((BGR24, w, h) =>
                {
                    _RemoteView = new Bitmap((int)w, (int)h, (int)w * 3, System.Drawing.Imaging.PixelFormat.Format24bppRgb, BGR24);
                    RefreshRemoteView();
                });
            }

            try
            {
                _ClientPeer.SocketConnect();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error Socket Location:{SocketUrl.Text}");
                _ClientPeer.Dispose();
                _ClientPeer = null;
            }
        }


        Timer _ViewCaptureTrigger;
        ScreenCaptureInfo _CaptureInfo = new ScreenCaptureInfo();
        ScreenCapturer _Capturer = new ScreenCapturer();
        Bitmap _ScreenView;
        private void CaptureBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_ViewCaptureTrigger != null)
            {
                _ViewCaptureTrigger.Enabled = !_ViewCaptureTrigger.Enabled;
                return;
            }

            _ViewCaptureTrigger = new Timer { Interval = 1000 / _CaptureInfo.CaptureFps, };
            _ViewCaptureTrigger.Elapsed += (s, o) =>
            {
                _ScreenView = _Capturer.CaptureView();
                RefreshLocalView();
            };
            _ViewCaptureTrigger.Enabled = true;

        }

        Timer _ViewPushTrigger;
        Bitmap _RemoteView;
        private void BuildRtc_Click(object sender, RoutedEventArgs e)
        {
            if (_ClientPeer == null) return;

            if (_ViewPushTrigger != null)
            {
                _ViewPushTrigger.Enabled = !_ViewPushTrigger.Enabled;
                return;
            }

            _ViewPushTrigger = new Timer { Interval = 1000 / _CaptureInfo.CaptureFps, };
            _ViewPushTrigger.Elapsed += (s, o) =>
            {
                _ScreenView = _Capturer.CaptureView();
                _ClientPeer._WebRtc.PushFrame(_Capturer.ImgBufPtr);
            };
            _ClientPeer.RtcConnect();
            _ViewPushTrigger.Enabled = true;
        }

        private void RefreshLocalView()
        {
            this.LocalView.Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    LocalView.Source = Helper.BitmapToBitmapSource(_ScreenView);
                }
                catch (Exception ex)
                {
                    Debug.Write($"Refresh Error:{ex.Message}");
                }
            }));
        }

        private void RefreshRemoteView()
        {
            this.RemoteView.Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    if (_RemoteView == null) return;
                    RemoteView.Source = Helper.BitmapToBitmapSource(_RemoteView);
                }
                catch (Exception ex)
                {
                    Debug.Write($"Refresh Error:{ex.Message}");
                }
            }));
        }

    }
}
