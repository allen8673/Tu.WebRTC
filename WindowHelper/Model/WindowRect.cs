using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowHelper.Model
{
    /// <summary>
    /// Window Rectangle
    /// </summary>
    internal struct WindowRect
    {
        int left;
        int top;
        int right;
        int bottom;

        public int Left
        {
            get { return this.left; }
        }

        public int Top
        {
            get { return this.top; }
        }

        public int Width
        {
            get { return right - left; }
        }

        public int Height
        {
            get { return bottom - top; }
        }

        public static implicit operator Rectangle(WindowRect rect)
        {
            return new Rectangle(rect.left, rect.top, rect.Width, rect.Height);
        }
    }
}
