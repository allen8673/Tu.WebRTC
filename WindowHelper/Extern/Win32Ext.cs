using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WindowHelper.Model;

namespace WindowHelper.Extern
{
    internal class Win32Ext
    {
        public const string PROGRAMMANAGER = "Program Manager";
        public const string RUNDLL = "RunDLL";

        public const uint WM_PAINT = 0x000F;

        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_LAYERED = 0x80000;
        public const int LWA_ALPHA = 0x2;

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, ShowWindowEnum flags);

        [DllImport("user32.dll")]
        public static extern uint SendMessage(IntPtr hWnd, uint msg, uint wParam, uint lParam);

        [DllImport("user32")]
        public static extern int GetWindowRect(IntPtr hWnd, ref WindowRect rect);

        [DllImport("user32")]
        public static extern int PrintWindow(IntPtr hWnd, IntPtr dc, uint flags);

        [DllImport("user32")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxCount);

        [DllImport("user32")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(IntPtr hWnd);

        public delegate bool EnumWindowsCallbackHandler(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsCallbackHandler lpEnumFunc, IntPtr lParam);

        [DllImport("user32")]
        public static extern int GetWindowLong(IntPtr hWnd, int index);

        [DllImport("user32")]
        public static extern int SetWindowLong(IntPtr hWnd, int index, int dwNewLong);

        [DllImport("user32")]
        public static extern int SetLayeredWindowAttributes(IntPtr hWnd, byte crey, byte alpha, int flags);

        [DllImport("user32")]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder name, int maxCount);

        [DllImport("user32")]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32")]
        public static extern IntPtr SetParent(IntPtr child, IntPtr newParent);

        [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool reDraw);

        [DllImport("user32.dll")]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref WindowInfo pwi);

        [DllImport("user32.dll")]
        public static extern bool BringWindowToTop(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern bool SwitchToThisWindow(IntPtr hwnd, bool fAltTab);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetTopWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        public delegate void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventProc lpfnWinEventProc, uint idProcess, uint idThread, uint dwflags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int UnhookWinEvent(IntPtr hwnd);
    }
}
