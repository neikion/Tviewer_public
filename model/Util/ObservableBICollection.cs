using System.Collections;
using System.Collections.Generic;
using Tviewer.Interfaces;

namespace Tviewer.model.Util
{
    /// <summary>
    /// ObservableCollection supporting Bidirectional Iterator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableBICollection<T> : System.Collections.ObjectModel.ObservableCollection<T>, IEnumerable<T>
    {
        public CommandCarrier? RequestNextContent
        {
            private get;
            set;
        }

        public ObservableBICollection() : base()
        {
        }

        public ObservableBICollection(CommandCarrier? requestNextContent) : base()
        {
            RequestNextContent = requestNextContent;
        }

        public ObservableBICollection(ICollection<T> values, CommandCarrier? requestNextContent) : base(values)
        {
            RequestNextContent = requestNextContent;
        }
        public new IIterator<T> GetEnumerator() => GetEnumerator(RequestNextContent);

        public IIterator<T> GetEnumerator(CommandCarrier? command) => new Iterator<T>(Items, command);

        public IIterator<T> GetEnumerator(CommandCarrier? command, int index) => new Iterator<T>(Items, command, index);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
