using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{

    class WindowList : List<WindowInfo>
    {
        public WindowList() { }
        public WindowList(IEnumerable<KeyValuePair<IntPtr, string>> collection)
            :base()
        {
            base.AddRange(collection.Select(i => new WindowInfo(i.Key, i.Value)));
        }
        public void AddRange(IEnumerable<KeyValuePair<IntPtr, string>> collection)
        {
            base.AddRange(collection.Select(i=>new WindowInfo(i.Key, i.Value)));
        }
    }

    class WindowInfo
    {
        public WindowInfo(IntPtr hWnd, string name)
        {
            Name = name;
            HandlingPtr = hWnd;
        }

        public string Name { get; private set; }

        public IntPtr HandlingPtr { get; private set; }

        public override string ToString()
        {
            return $"{HandlingPtr} : {Name}";
        }
    }
}
