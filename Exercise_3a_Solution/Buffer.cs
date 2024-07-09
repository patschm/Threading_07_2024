using System.Threading;

namespace Problem_Shared_Array
{
    internal class Buffer
    {
        private readonly int[] _buffer;
        private int _bufferCounter = 0;
        private readonly SemaphoreSlim _semEmpty;
        private readonly SemaphoreSlim _semFull;


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
        public void Write(int value)
        {
            _semEmpty.Wait();
            _buffer[_bufferCounter] = value;
            Interlocked.Increment(ref _bufferCounter);
            _semFull.Release();
        }
        public int Read()
        {
            int nr = -100000;

            _semFull.Wait();
            Interlocked.Decrement(ref _bufferCounter);
            nr = _buffer[_bufferCounter];
            _semEmpty.Release();
            return nr;
        }
    }
}
