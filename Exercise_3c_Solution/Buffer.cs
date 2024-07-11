using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Problem_Shared_Data_ProdCons
{
    internal class Buffer : IProducerConsumerCollection<int>
    {
        private readonly int[] _buffer;
        private int _bufferCounter = 0;
        private readonly SemaphoreSlim _semEmpty;
        private readonly SemaphoreSlim _semFull;
        private static object _lock = new object();

        public Buffer(ushort size = 10)
        {
            if (size < 0)
            {
                size = 10;
            }
            _buffer = new int[size];
            _semEmpty = new SemaphoreSlim(size, size);
            _semFull = new SemaphoreSlim(0, size);
        }
         private void Write(int value)
        {
            _semEmpty.Wait();
            _buffer[_bufferCounter] = value;
            Interlocked.Increment(ref _bufferCounter);
            _semFull.Release();

        }
        private int Read()
        {
            int nr = -100000;

            _semFull.Wait();
            Interlocked.Decrement(ref _bufferCounter);
            nr = _buffer[_bufferCounter];
            _semEmpty.Release();
            return nr;
        }

        public int Count => _bufferCounter;
        public bool IsSynchronized => true;
        public object SyncRoot => _lock;

        public void CopyTo(int[] array, int index)
        {
            lock (_lock)
            {
                _buffer.CopyTo(array, index);
            }
        }

        public int[] ToArray()
        {
            int[] ret;
            lock (_lock)
            {
                ret = _buffer.ToArray();
            }
            return ret;
        }

        public bool TryAdd(int item)
        {
            Write(item);
            return true;
        }

        public bool TryTake([MaybeNullWhen(false)] out int item)
        {
            item = Read();
            return true;
        }

        public IEnumerator<int> GetEnumerator()
        {
            int[] ret = [];
            lock (_lock)
            {
                _buffer.CopyTo(_buffer, _buffer.Length);
            }
            return (IEnumerator<int>)ret.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            CopyTo(array, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
