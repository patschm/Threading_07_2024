using System.Threading;

namespace Problem_Shared_Array
{
    // TODO: Run this app and observe it's behavior.
    // Make this collection thread-safe
    internal class Buffer
    {
        private readonly int[] _buffer;
        private int _bufferCounter = 0;
       
        public Buffer(ushort size = 10)
        {
            if (size < 0)
            {
                size = 10;
            }
            _buffer = new int[size];
        }
        public void Write(int value)
        {
            _buffer[_bufferCounter] = value;
            Interlocked.Increment(ref _bufferCounter);
        }
        public int Read()
        {
            int nr = -100000;

            Interlocked.Decrement(ref _bufferCounter);
            nr = _buffer[_bufferCounter];
            return nr;
        }
    }
}
