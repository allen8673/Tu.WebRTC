using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowCapture
{
    public class WindowSnap
    {
        #region Static
        /// <summary>
        /// if is true ,will force the mdi child to be captured completely ,maybe incompatible with some programs
        /// </summary>
        public static bool ForceMDICapturing
        {
            get { return _ForceMDI; }
            set { _ForceMDI = value; }
        }
        private static bool _ForceMDI = true;

        [ThreadStatic]
        private static bool countMinimizedWindows;

        [ThreadStatic]
        private static bool useSpecialCapturing;

        [ThreadStatic]
        private static WindowSnapCollection windowSnaps;

        [ThreadStatic]
        private static int winLong;

        [ThreadStatic]
        private static bool minAnimateChanged = false;

        private static bool EnumWindowsCallback(IntPtr hWnd, IntPtr lParam)
        {
            bool specialCapturing = false;
            if (hWnd == IntPtr.Zero) return false;
            if (!Win32Ext.IsWindowVisible(hWnd)) return true;
            if (!countMinimizedWindows)
            {
                if (Win32Ext.IsIconic(hWnd)) return true;
            }
            else if (Win32Ext.IsIconic(hWnd) && useSpecialCapturing) specialCapturing = true;

            if (GetWindowText(hWnd) == Win32Ext.PROGRAMMANAGER) return true;

            var Snap = new WindowSnap(hWnd, specialCapturing);

            if (!Snap.Text.Equals(""))
                windowSnaps.Add(Snap);

            return true;
        }

        /// <summary>
        /// Get the collection of WindowSnap instances fro all available windows
        /// </summary>
        /// <param name="minimized">Capture a window even it's Minimized</param>
        /// <param name="specialCapturring">use special capturing method to capture minmized windows</param>
        /// <returns>return collections of WindowSnap instances</returns>
        public static WindowSnapCollection GetAllWindows(bool minimized, bool specialCapturring)
        {
            windowSnaps = new WindowSnapCollection();
            countMinimizedWindows = minimized;//set minimized flag capture
            useSpecialCapturing = specialCapturring;//set specialcapturing flag

            Win32Ext.EnumWindowsCallbackHandler callback = new Win32Ext.EnumWindowsCallbackHandler(EnumWindowsCallback);
            Win32Ext.EnumWindows(callback, IntPtr.Zero);

            return new WindowSnapCollection(windowSnaps.ToArray(), true);
        }

        /// <summary>
        /// Get the collection of WindowSnap instances fro all available windows
        /// </summary>
        /// <returns>return collections of WindowSnap instances</returns>
        public static WindowSnapCollection GetAllWindows()
        {
            return GetAllWindows(false, false);
        }

        /// <summary>
        /// Take a Snap from the specific Window
        /// </summary>
        /// <param name="hWnd">Handle of the Window</param>
        /// <param name="useSpecialCapturing">if you need to capture from the minimized windows set it true,otherwise false</param>
        /// <returns></returns>
        public static WindowSnap GetWindowSnap(IntPtr hWnd, bool useSpecialCapturing)
        {
            if (!useSpecialCapturing)
                return new WindowSnap(hWnd, false);
            return new WindowSnap(hWnd, NeedSpecialCapturing(hWnd));
        }

        private static bool NeedSpecialCapturing(IntPtr hWnd)
        {
            if (Win32Ext.IsIconic(hWnd)) return true;
            return false;
        }

        private static Bitmap GetWindowImage(IntPtr hWnd, Size size)
        {
            try
            {
                if (size.IsEmpty || size.Height < 0 || size.Width < 0) return null;
                Bitmap bmp = new Bitmap(size.Width, size.Height);
                Graphics g = Graphics.FromImage(bmp);
                IntPtr dc = g.GetHdc();
                Win32Ext.PrintWindow(hWnd, dc, 0);
                g.ReleaseHdc();
                g.Dispose();
                return bmp;
            }
            catch { return null; }
        }

        private static string GetWindowText(IntPtr hWnd)
        {
            int length = Win32Ext.GetWindowTextLength(hWnd) + 1;
            StringBuilder name = new StringBuilder(length);
            Win32Ext.GetWindowText(hWnd, name, length);
            return name.ToString();
        }

        private static Rectangle GetWindowPlacement(IntPtr hWnd)
        {
            WindowRect rect = new WindowRect();
            Win32Ext.GetWindowRect(hWnd, ref rect);
            return rect;
        }

        private static void ExitSpecialCapturing(IntPtr hWnd)
        {
            Win32Ext.ShowWindow(hWnd, ShowWindowEnum.Minimize);
            Win32Ext.SetWindowLong(hWnd, Win32Ext.GWL_EXSTYLE, winLong);

            if (minAnimateChanged)
            {
                XPAppearance.MinAnimate = true;
                minAnimateChanged = false;
            }

        }
        #endregion

        #region Properties
        private Capture _Capture = new Capture();
        public Capture Capture => _Capture;

        /// <summary>
        /// Get the Captured Image of the Window
        /// </summary>
        public Bitmap Image { get; private set; }

        /// <summary>
        /// Get Size of Snapped Window
        /// </summary>
        public Size Size { get; private set; }

        /// <summary>
        /// Get Location of Snapped Window
        /// </summary>
        public Point Location { get; private set; }

        /// <summary>
        /// Get Title of Snapped Window
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Get Handle of Snapped Window
        /// </summary>
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// if the state of the window is minimized return true otherwise returns false
        /// </summary>
        public bool IsMinimized { get; private set; }
        #endregion

        #region Constructor
        private WindowSnap(IntPtr hWnd, bool specialCapturing)
        {
            Rectangle rect = GetWindowPlacement(hWnd);

            {
                this.IsMinimized = Win32Ext.IsIconic(hWnd);
                this.Handle = hWnd;
                this.Size = rect.Size;
                this.Location = rect.Location;
                this.Text = GetWindowText(hWnd);
                this.Image = GetWindowImage(hWnd, this.Size);
            }

            ChildSupport(hWnd);

            if (specialCapturing) EnterSpecialCapturing(hWnd);
        }

        /// <summary>
        /// Child Window Support
        /// </summary>
        /// <param name="hWnd"></param>
        private void ChildSupport(IntPtr hWnd)
        {
            #region Condition initialize
            WindowInfo wInfo = new WindowInfo()
            {
                cbSize = WindowInfo.GetSize()
            };
            Win32Ext.GetWindowInfo(hWnd, ref wInfo);
            IntPtr parent = Win32Ext.GetParent(hWnd);
            #endregion
            #region Condition
            if (!ForceMDICapturing || parent == IntPtr.Zero ||
                   (wInfo.dwExStyle & ExtendedWindowStyles.WS_EX_MDICHILD) != ExtendedWindowStyles.WS_EX_MDICHILD) return;

            StringBuilder name = new StringBuilder();
            Win32Ext.GetClassName(parent, name, Win32Ext.RUNDLL.Length + 1);

            if (name.ToString() == Win32Ext.RUNDLL) return;
            #endregion
            #region Child Support
            Rectangle pos = GetWindowPlacement(hWnd);
            Win32Ext.MoveWindow(hWnd, int.MaxValue, int.MaxValue, pos.Width, pos.Height, true);
            Win32Ext.SetParent(hWnd, IntPtr.Zero);
            Win32Ext.SetParent(hWnd, parent);
            Rectangle parentPos = GetWindowPlacement(parent);
            int x = wInfo.rcWindow.Left - parentPos.X;
            int y = wInfo.rcWindow.Top - parentPos.Y;
            if ((wInfo.dwStyle & WindowStyles.WS_THICKFRAME) == WindowStyles.WS_THICKFRAME)
            {
                x -= SystemInformation.Border3DSize.Width;
                y -= SystemInformation.Border3DSize.Height;
            }
            Win32Ext.MoveWindow(hWnd, x, y, pos.Width, pos.Height, true);
            #endregion
        }

        private void EnterSpecialCapturing(IntPtr hWnd)
        {
            if (XPAppearance.MinAnimate)
            {
                XPAppearance.MinAnimate = false;
                minAnimateChanged = true;
            }

            winLong = Win32Ext.GetWindowLong(hWnd, Win32Ext.GWL_EXSTYLE);
            Win32Ext.SetWindowLong(hWnd, Win32Ext.GWL_EXSTYLE, winLong | Win32Ext.WS_EX_LAYERED);
            Win32Ext.SetLayeredWindowAttributes(hWnd, 0, 1, Win32Ext.LWA_ALPHA);
            Win32Ext.ShowWindow(hWnd, ShowWindowEnum.Restore);
            Win32Ext.SendMessage(hWnd, Win32Ext.WM_PAINT, 0, 0);
        } 
        #endregion
    }
}
