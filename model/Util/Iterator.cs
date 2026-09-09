using System.Collections;
using System.Collections.Generic;
using Tviewer.Interfaces;

namespace Tviewer.model.Util
{
    public class Iterator<T> : IIterator<T>
    {
        private IList<T> Items;
        private CommandCarrier? RequestNextContent;
        private int currentIndex;

        public Iterator(IList<T> list, CommandCarrier? requestNextContent, int Index = -1)
        {
            Items = list;
            RequestNextContent = requestNextContent;
            currentIndex = Index;
        }

        public T Current
        {
            get
            {
                if (currentIndex < 0 || currentIndex >= Items.Count)
                {
                    throw new System.InvalidOperationException();
                }
                return Items[currentIndex];
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            Items = null;
            RequestNextContent = null;
            currentIndex = -1;
        }

        public bool MoveNext()
        {
            if (currentIndex + 1 == Items.Count)
            {
                RequestNextContent?.Execute();
            }
            if (currentIndex + 1 < Items.Count)
            {
                currentIndex++;
                return true;
            }
            return false;
        }
        public bool MovePrev()
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            currentIndex = 0;
        }
    }
}
