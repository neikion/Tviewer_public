using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WPF_Practice.model
{
    public class BufferWaitList<T> : WaitList<T>
    {
        private int _bufferSize;
        public int BufferSize { get { return _bufferSize; } }
        public BufferWaitList() : this(10)
        {
        }
        /// <summary>
        /// 입력한 크기의 버퍼를 3개 만들어 사용
        /// </summary>
        /// <param name="BufferSize"></param>
        public BufferWaitList(int BufferSize) : base(BufferSize*3)
        {
            this._bufferSize = BufferSize;
        }
    }
}
