﻿using System;
using System.Threading;

namespace Problem_Shared_Array
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Buffer buffer = new Buffer();
           
            const int NR_READERS = 1;
            const int NR_WRITERS = 2;

            for (int i = 0; i < NR_WRITERS; i++)
                new NumberWriter(buffer).Run();

            for (int i = 1; i < NR_READERS; i++)
                new NumberReader(buffer).Run();
            
            NumberReader reader = new NumberReader(buffer);
            var t2 = reader.Run();
            t2.Join();

            Console.WriteLine("End Program");
            Console.ReadLine();
        }
    }


}
