using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowHelper.Model
{
    /// <summary>
    /// ANIMATIONINFO specifies animation effects associated with user actions. 
    /// Used with SystemParametersInfo when SPI_GETANIMATION or SPI_SETANIMATION action is specified.
    /// </summary>
    /// <remark>
    /// The uiParam value must be set to (System.UInt32)Marshal.SizeOf(typeof(ANIMATIONINFO)) when using this structure.
    /// </remark>
    [StructLayout(LayoutKind.Sequential)]
    internal struct AnimationInfo
    {
        /// <summary>
        /// Creates an AMINMATIONINFO structure.
        /// </summary>
        /// <param name="iMinAnimate">If non-zero and SPI_SETANIMATION is specified, enables minimize/restore animation.</param>
        public AnimationInfo(bool iMinAnimate)
        {
            this.cbSize = GetSize();

            if (iMinAnimate) this.iMinAnimate = 1;
            else this.iMinAnimate = 0;
        }

        /// <summary>
        /// Always must be set to (System.UInt32)Marshal.SizeOf(typeof(ANIMATIONINFO)).
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// If non-zero, minimize/restore animation is enabled, otherwise disabled.
        /// </summary>
        private int iMinAnimate;

        public bool IMinAnimate
        {
            get
            {
                if (this.iMinAnimate == 0) return false;
                else return true;
            }
            set
            {
                if (value == true) this.iMinAnimate = 1;
                else this.iMinAnimate = 0;
            }
        }

        public static uint GetSize()
        {
            return (uint)Marshal.SizeOf(typeof(AnimationInfo));
        }
    }
}
