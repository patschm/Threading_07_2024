using System.Collections.Concurrent;

namespace Problem_Shared_Stack
{
    internal class NumberWriter
    {
        private readonly ConcurrentStack<int> _buffer;
        
        public NumberWriter(ConcurrentStack<int> buffer)
        {
            _buffer = buffer;
        }
        public Thread Run()
        {
            var t1 = new Thread(Writer);
            t1.Start();
            return t1;
        }
        private void Writer()
        {
            var rnd = new Random();   
            while (true)
            {
                int nr = rnd.Next(1, 100);
               _buffer.Push(nr);
            }
        }
    }
}
