using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowHelper.Model
{
    internal static class XPAppearance
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, ref AnimationInfo pvParam, SPIF fWinIni);

        /// <summary>
        /// Gets or Sets MinAnimate Effect
        /// </summary>
        public static bool MinAnimate
        {
            get
            {
                AnimationInfo animationInfo = new AnimationInfo(false);

                SystemParametersInfo(SPI.SPI_GETANIMATION, AnimationInfo.GetSize(), ref animationInfo, SPIF.None);
                return animationInfo.IMinAnimate;
            }
            set
            {
                AnimationInfo animationInfo = new AnimationInfo(value);
                SystemParametersInfo(SPI.SPI_SETANIMATION, AnimationInfo.GetSize(), ref animationInfo, SPIF.SPIF_SENDCHANGE);
            }
        }

        #region Const
        private const uint SPI_GETBEEP = 0x0001;
        private const uint SPI_SETBEEP = 0x0002;
        private const uint SPI_GETMOUSE = 0x0003;
        private const uint SPI_SETMOUSE = 0x0004;
        private const uint SPI_GETBORDER = 0x0005;
        private const uint SPI_SETBORDER = 0x0006;
        private const uint SPI_GETKEYBOARDSPEED = 0x000A;
        private const uint SPI_SETKEYBOARDSPEED = 0x000B;
        private const uint SPI_LANGDRIVER = 0x000C;
        private const uint SPI_ICONHORIZONTALSPACING = 0x000D;
        private const uint SPI_GETSCREENSAVETIMEOUT = 0x000E;
        private const uint SPI_SETSCREENSAVETIMEOUT = 0x000F;
        private const uint SPI_GETSCREENSAVEACTIVE = 0x0010;
        private const uint SPI_SETSCREENSAVEACTIVE = 0x0011;
        private const uint SPI_GETGRIDGRANULARITY = 0x0012;
        private const uint SPI_SETGRIDGRANULARITY = 0x0013;
        private const uint SPI_SETDESKWALLPAPER = 0x0014;
        private const uint SPI_SETDESKPATTERN = 0x0015;
        private const uint SPI_GETKEYBOARDDELAY = 0x0016;
        private const uint SPI_SETKEYBOARDDELAY = 0x0017;
        private const uint SPI_ICONVERTICALSPACING = 0x0018;
        private const uint SPI_GETICONTITLEWRAP = 0x0019;
        private const uint SPI_SETICONTITLEWRAP = 0x001A;
        private const uint SPI_GETMENUDROPALIGNMENT = 0x001B;
        private const uint SPI_SETMENUDROPALIGNMENT = 0x001C;
        private const uint SPI_SETDOUBLECLKWIDTH = 0x001D;
        private const uint SPI_SETDOUBLECLKHEIGHT = 0x001E;
        private const uint SPI_GETICONTITLELOGFONT = 0x001F;
        private const uint SPI_SETDOUBLECLICKTIME = 0x0020;
        private const uint SPI_SETMOUSEBUTTONSWAP = 0x0021;
        private const uint SPI_SETICONTITLELOGFONT = 0x0022;
        private const uint SPI_GETFASTTASKSWITCH = 0x0023;
        private const uint SPI_SETFASTTASKSWITCH = 0x0024;
        private const uint SPI_SETDRAGFULLWINDOWS = 0x0025;
        private const uint SPI_GETDRAGFULLWINDOWS = 0x0026;
        private const uint SPI_GETNONCLIENTMETRICS = 0x0029;
        private const uint SPI_SETNONCLIENTMETRICS = 0x002A;
        private const uint SPI_GETMINIMIZEDMETRICS = 0x002B;
        private const uint SPI_SETMINIMIZEDMETRICS = 0x002C;
        private const uint SPI_GETICONMETRICS = 0x002D;
        private const uint SPI_SETICONMETRICS = 0x002E;
        private const uint SPI_SETWORKAREA = 0x002F;
        private const uint SPI_GETWORKAREA = 0x0030;
        private const uint SPI_SETPENWINDOWS = 0x0031;
        private const uint SPI_GETHIGHCONTRAST = 0x0042;
        private const uint SPI_SETHIGHCONTRAST = 0x0043;
        private const uint SPI_GETKEYBOARDPREF = 0x0044;
        private const uint SPI_SETKEYBOARDPREF = 0x0045;
        private const uint SPI_GETSCREENREADER = 0x0046;
        private const uint SPI_SETSCREENREADER = 0x0047;
        private const uint SPI_GETANIMATION = 0x0048;
        private const uint SPI_SETANIMATION = 0x0049;
        private const uint SPI_GETFONTSMOOTHING = 0x004A;
        private const uint SPI_SETFONTSMOOTHING = 0x004B;
        private const uint SPI_SETDRAGWIDTH = 0x004C;
        private const uint SPI_SETDRAGHEIGHT = 0x004D;
        private const uint SPI_SETHANDHELD = 0x004E;
        private const uint SPI_GETLOWPOWERTIMEOUT = 0x004F;
        private const uint SPI_GETPOWEROFFTIMEOUT = 0x0050;
        private const uint SPI_SETLOWPOWERTIMEOUT = 0x0051;
        private const uint SPI_SETPOWEROFFTIMEOUT = 0x0052;
        private const uint SPI_GETLOWPOWERACTIVE = 0x0053;
        private const uint SPI_GETPOWEROFFACTIVE = 0x0054;
        private const uint SPI_SETLOWPOWERACTIVE = 0x0055;
        private const uint SPI_SETPOWEROFFACTIVE = 0x0056;
        private const uint SPI_SETICONS = 0x0058;
        private const uint SPI_GETDEFAULTINPUTLANG = 0x0059;
        private const uint SPI_SETDEFAULTINPUTLANG = 0x005A;
        private const uint SPI_SETLANGTOGGLE = 0x005B;
        private const uint SPI_GETWINDOWSEXTENSION = 0x005C;
        private const uint SPI_SETMOUSETRAILS = 0x005D;
        private const uint SPI_GETMOUSETRAILS = 0x005E;
        private const uint SPI_SCREENSAVERRUNNING = 0x0061;
        private const uint SPI_GETFILTERKEYS = 0x0032;
        private const uint SPI_SETFILTERKEYS = 0x0033;
        private const uint SPI_GETTOGGLEKEYS = 0x0034;
        private const uint SPI_SETTOGGLEKEYS = 0x0035;
        private const uint SPI_GETMOUSEKEYS = 0x0036;
        private const uint SPI_SETMOUSEKEYS = 0x0037;
        private const uint SPI_GETSHOWSOUNDS = 0x0038;
        private const uint SPI_SETSHOWSOUNDS = 0x0039;
        private const uint SPI_GETSTICKYKEYS = 0x003A;
        private const uint SPI_SETSTICKYKEYS = 0x003B;
        private const uint SPI_GETACCESSTIMEOUT = 0x003C;
        private const uint SPI_SETACCESSTIMEOUT = 0x003D;
        private const uint SPI_GETSERIALKEYS = 0x003E;
        private const uint SPI_SETSERIALKEYS = 0x003F;
        private const uint SPI_GETSOUNDSENTRY = 0x0040;
        private const uint SPI_SETSOUNDSENTRY = 0x0041;
        private const uint SPI_GETSNAPTODEFBUTTON = 0x005F;
        private const uint SPI_SETSNAPTODEFBUTTON = 0x0060;
        private const uint SPI_GETMOUSEHOVERWIDTH = 0x0062;
        private const uint SPI_SETMOUSEHOVERWIDTH = 0x0063;
        private const uint SPI_GETMOUSEHOVERHEIGHT = 0x0064;
        private const uint SPI_SETMOUSEHOVERHEIGHT = 0x0065;
        private const uint SPI_GETMOUSEHOVERTIME = 0x0066;
        private const uint SPI_SETMOUSEHOVERTIME = 0x0067;
        private const uint SPI_GETWHEELSCROLLLINES = 0x0068;
        private const uint SPI_SETWHEELSCROLLLINES = 0x0069;
        private const uint SPI_GETMENUSHOWDELAY = 0x006A;
        private const uint SPI_SETMENUSHOWDELAY = 0x006B;
        private const uint SPI_GETSHOWIMEUI = 0x006E;
        private const uint SPI_SETSHOWIMEUI = 0x006F;
        private const uint SPI_GETMOUSESPEED = 0x0070;
        private const uint SPI_SETMOUSESPEED = 0x0071;
        private const uint SPI_GETSCREENSAVERRUNNING = 0x0072;
        private const uint SPI_GETDESKWALLPAPER = 0x0073;
        private const uint SPI_GETACTIVEWINDOWTRACKING = 0x1000;
        private const uint SPI_SETACTIVEWINDOWTRACKING = 0x1001;
        private const uint SPI_GETMENUANIMATION = 0x1002;
        private const uint SPI_SETMENUANIMATION = 0x1003;
        private const uint SPI_GETCOMBOBOXANIMATION = 0x1004;
        private const uint SPI_SETCOMBOBOXANIMATION = 0x1005;
        private const uint SPI_GETLISTBOXSMOOTHSCROLLING = 0x1006;
        private const uint SPI_SETLISTBOXSMOOTHSCROLLING = 0x1007;
        private const uint SPI_GETGRADIENTCAPTIONS = 0x1008;
        private const uint SPI_SETGRADIENTCAPTIONS = 0x1009;
        private const uint SPI_GETKEYBOARDCUES = 0x100A;
        private const uint SPI_SETKEYBOARDCUES = 0x100B;
        private const uint SPI_GETMENUUNDERLINES = SPI_GETKEYBOARDCUES;
        private const uint SPI_SETMENUUNDERLINES = SPI_SETKEYBOARDCUES;
        private const uint SPI_GETACTIVEWNDTRKZORDER = 0x100C;
        private const uint SPI_SETACTIVEWNDTRKZORDER = 0x100D;
        private const uint SPI_GETHOTTRACKING = 0x100E;
        private const uint SPI_SETHOTTRACKING = 0x100F;
        private const uint SPI_GETMENUFADE = 0x1012;
        private const uint SPI_SETMENUFADE = 0x1013;
        private const uint SPI_GETSELECTIONFADE = 0x1014;
        private const uint SPI_SETSELECTIONFADE = 0x1015;
        private const uint SPI_GETTOOLTIPANIMATION = 0x1016;
        private const uint SPI_SETTOOLTIPANIMATION = 0x1017;
        private const uint SPI_GETTOOLTIPFADE = 0x1018;
        private const uint SPI_SETTOOLTIPFADE = 0x1019;
        private const uint SPI_GETCURSORSHADOW = 0x101A;
        private const uint SPI_SETCURSORSHADOW = 0x101B;
        private const uint SPI_GETMOUSESONAR = 0x101C;
        private const uint SPI_SETMOUSESONAR = 0x101D;
        private const uint SPI_GETMOUSECLICKLOCK = 0x101E;
        private const uint SPI_SETMOUSECLICKLOCK = 0x101F;
        private const uint SPI_GETMOUSEVANISH = 0x1020;
        private const uint SPI_SETMOUSEVANISH = 0x1021;
        private const uint SPI_GETFLATMENU = 0x1022;
        private const uint SPI_SETFLATMENU = 0x1023;
        private const uint SPI_GETDROPSHADOW = 0x1024;
        private const uint SPI_SETDROPSHADOW = 0x1025;
        private const uint SPI_GETBLOCKSENDINPUTRESETS = 0x1026;
        private const uint SPI_SETBLOCKSENDINPUTRESETS = 0x1027;
        private const uint SPI_GETUIEFFECTS = 0x103E;
        private const uint SPI_SETUIEFFECTS = 0x103F;
        private const uint SPI_GETFOREGROUNDLOCKTIMEOUT = 0x2000;
        private const uint SPI_SETFOREGROUNDLOCKTIMEOUT = 0x2001;
        private const uint SPI_GETACTIVEWNDTRKTIMEOUT = 0x2002;
        private const uint SPI_SETACTIVEWNDTRKTIMEOUT = 0x2003;
        private const uint SPI_GETFOREGROUNDFLASHCOUNT = 0x2004;
        private const uint SPI_SETFOREGROUNDFLASHCOUNT = 0x2005;
        private const uint SPI_GETCARETWIDTH = 0x2006;
        private const uint SPI_SETCARETWIDTH = 0x2007;
        private const uint SPI_GETMOUSECLICKLOCKTIME = 0x2008;
        private const uint SPI_SETMOUSECLICKLOCKTIME = 0x2009;
        private const uint SPI_GETFONTSMOOTHINGTYPE = 0x200A;
        private const uint SPI_SETFONTSMOOTHINGTYPE = 0x200B;
        private const uint SPI_GETFONTSMOOTHINGCONTRAST = 0x200C;
        private const uint SPI_SETFONTSMOOTHINGCONTRAST = 0x200D;
        private const uint SPI_GETFOCUSBORDERWIDTH = 0x200E;
        private const uint SPI_SETFOCUSBORDERWIDTH = 0x200F;
        private const uint SPI_GETFOCUSBORDERHEIGHT = 0x2010;
        private const uint SPI_SETFOCUSBORDERHEIGHT = 0x2011;
        private const uint SPI_GETFONTSMOOTHINGORIENTATION = 0x2012;
        private const uint SPI_SETFONTSMOOTHINGORIENTATION = 0x2013;
        #endregion
    }
}
