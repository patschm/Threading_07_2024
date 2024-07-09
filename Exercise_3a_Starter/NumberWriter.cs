using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Problem_Shared_Array
{
    internal class NumberWriter
    {
        private readonly Buffer _buffer;
        
        public NumberWriter(Buffer buffer)
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
                _buffer.Write(nr);
            }
        }
    }
}
