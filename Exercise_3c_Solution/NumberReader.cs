using System;
using System.Threading;

namespace Problem_Shared_Data_ProdCons
{
    internal class NumberReader
    {
        private readonly Buffer _buffer;

        public NumberReader(Buffer buffer)
        {
            _buffer = buffer;
        }
        public Thread Run()
        {
            var t1 = new Thread(Reader);
            t1.Start();
            return t1;
        }

        private void Reader()
        {
            while (true)
            {
                //int nr = _buffer.Read();
                 _buffer.TryTake(out int nr);
                if (nr < 0)
                {
                    Console.WriteLine($"\t\t\t\t\t\t{nr}");
                }
                else
                {
                    Console.WriteLine($"{nr}");
                }
            }
        }
    }
}
