using System;
using System.IO;

namespace Tviewer.model.Util
{
    internal class CharArrayStreamReader : TextReader
    {
        char[]? store;
        int pos = 0;

        public bool IsNull { get => store is null; }

        public void SetValue(char[] array)
        {
            pos = 0;
            store = array;
        }

        public override int Read()
        {
            if (pos < store?.Length)
            {
                return store[pos++];
            }
            else
            {
                store = null;
                return -1;
            }
        }

        public override int Read(char[] buffer, int index, int count)
        {
            if (store is null || pos >= store.Length || index >= store.Length || buffer.Length < pos)
            {
                return -1;
            }
            int n = Math.Min(count, store.Length - pos);
            store.AsSpan().Slice(index, n).CopyTo(buffer);
            pos += n;
            return n;
        }

        public override string ReadToEnd()
        {
            return store.AsSpan().Slice(pos).ToString();
        }

        public void Clear()
        {
            if (store is not null) pos = store.Length;
            store = null;
        }

        protected override void Dispose(bool disposing)
        {
            Clear();
            base.Dispose(disposing);
        }
    }
}
