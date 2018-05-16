using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowHelper.Model
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowInfo
    {
        public uint cbSize;
        public WindowRect rcWindow;
        public WindowRect rcClient;
        public WindowStyles dwStyle;
        public ExtendedWindowStyles dwExStyle;
        public uint dwWindowStatus;
        public uint cxWindowBorders;
        public uint cyWindowBorders;
        public ushort atomWindowType;
        public ushort wCreatorVersion;

        public static uint GetSize()
        {
            return (uint)Marshal.SizeOf(typeof(WindowInfo));
        }
    }
}
