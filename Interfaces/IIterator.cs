using System;
using System.Collections;

namespace Tviewer.Interfaces
{
    /// <summary>
    /// Bidirectional Iterator
    /// </summary>
    public interface IIterator : IEnumerator
    {
        public bool MovePrev();
    }

    /// <summary>
    /// Bidirectional Iterator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIterator<out T> : IIterator, IDisposable
    {
        new T Current { get; }
    }
}
