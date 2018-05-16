using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowHelper
{
    public class WindowSnapCollection : List<WindowSnap>
    {
        private readonly bool asReadonly = false;

        const string READONLYEXCEPTIONTEXT = "The Collection is marked as Read-Only and it cannot be modified";
        void ThrowAReadonlyException()
        {
            throw new Exception(READONLYEXCEPTIONTEXT);
        }

        public new void Add(WindowSnap item)
        {
            if (this.asReadonly) this.ThrowAReadonlyException();
            base.Add(item);
        }

        public new void AddRange(IEnumerable<WindowSnap> collection)
        {
            if (this.asReadonly) this.ThrowAReadonlyException();
            base.AddRange(collection);
        }

        public new void Clear()
        {
            if (this.asReadonly) this.ThrowAReadonlyException();
            base.Clear();
        }

        public new void Insert(int index, WindowSnap item)
        {
            if (this.asReadonly) this.ThrowAReadonlyException();
            base.Insert(index, item);
        }

        public new void InsertRange(int index, IEnumerable<WindowSnap> collection)
        {
            if (this.asReadonly) this.ThrowAReadonlyException();
            base.InsertRange(index, collection);
        }

        public new void Remove(WindowSnap item)
        {
            if (this.asReadonly) this.ThrowAReadonlyException();
            base.Remove(item);
        }

        public new void RemoveAll(Predicate<WindowSnap> match)
        {
            if (this.asReadonly) this.ThrowAReadonlyException();
            base.RemoveAll(match);
        }

        public new void RemoveAt(int index)
        {
            if (this.asReadonly) this.ThrowAReadonlyException();
            base.RemoveAt(index);
        }

        public new void RemoveRange(int index, int count)
        {
            if (this.asReadonly) this.ThrowAReadonlyException();
            base.RemoveRange(index, count);
        }

        public new void Reverse(int index, int count)
        {
            if (this.asReadonly) this.ThrowAReadonlyException();
            base.Reverse(index, count);
        }

        public new void Reverse()
        {
            if (this.asReadonly) this.ThrowAReadonlyException();
            base.Reverse();
        }

        public new void Sort()
        {
            if (this.asReadonly) this.ThrowAReadonlyException();
            base.Sort();
        }

        public new void Sort(Comparison<WindowSnap> comparison)
        {
            if (this.asReadonly) this.ThrowAReadonlyException();
            base.Sort(comparison);
        }

        public new void Sort(IComparer<WindowSnap> compare)
        {
            if (this.asReadonly) this.ThrowAReadonlyException();
            base.Sort(compare);
        }

        public new void Sort(int index, int count, IComparer<WindowSnap> compare)
        {
            if (this.asReadonly) this.ThrowAReadonlyException();
            base.Sort(index, count, compare);
        }


        public bool Contains(IntPtr hWnd)
        {
            if (GetWindowSnap(hWnd) != null) return true;
            return false;

        }

        public WindowSnap GetWindowSnap(IntPtr hWnd)
        {
            checkHWnd = hWnd;
            return base.Find(IshWndPredict);
        }

        public void Update(WindowSnap item)
        {
            lock (this)
            {
                WindowSnap oldSnap = this.GetWindowSnap(item.Handle);
                this.Remove(oldSnap);
                this.Add(item);
            }
        }

        public WindowSnapCollection GetAllMinimized()
        {
            WindowSnapCollection wsCol = (WindowSnapCollection)base.FindAll(IsMinimizedPredict);
            return wsCol;
        }

        static bool IsMinimizedPredict(WindowSnap ws)
        {
            if (ws.IsMinimized) return true;
            return false;
        }

        [ThreadStatic] private static IntPtr checkHWnd;
        static bool IshWndPredict(WindowSnap ws)
        {
            if (ws.Handle == checkHWnd)
                return true;
            return false;
        }

        bool ReadOnly
        {
            get { return this.asReadonly; }
        }

        internal WindowSnapCollection(WindowSnap[] items, bool asReadOnly)
        {
            base.AddRange(items);
            base.TrimExcess();
            this.asReadonly = asReadOnly;
        }

        internal WindowSnapCollection()
        {
            this.asReadonly = false;
        }
    }
}
