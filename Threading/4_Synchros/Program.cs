using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading;
using System.Threading.Tasks;

namespace M4_Collections
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Problem_1();
            //Solution_1();
            Blocking_Collection();
            //Concurrent_Bag();
            //Concurrent_Stack();
            // Exercise 3b & 3c
            Console.ReadLine();
        }

        private static void Problem_1()
        {
            var rnd = new Random();
            var cde = new CountdownEvent(3);
            
            var list = new List<int>();

            for (int j = 0; j < 3; j++)
            {
                new Thread(() =>
                {
                    for (int i = 0; i < 20; i++)
                    {
                        int nr = list.LastOrDefault();
                        Thread.Sleep(10);
                        list.Add(++nr);
                    }
                    cde.Signal();
                }).Start();
            }
            cde.Wait();

            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }

        static object locker = new object();
        private static void Solution_1()
        {
            var rnd = new Random();
            var cde = new CountdownEvent(3);

            var list = new List<int>();

            for (int j = 0; j < 3; j++)
            {
                new Thread(() =>
                {
                    for (int i = 0; i < 20; i++)
                    {
                        lock (locker)
                        {
                            int nr = list.LastOrDefault();
                            Thread.Sleep(10);
                            list.Add(++nr);
                        }
                    }
                    cde.Signal();
                }).Start();
            }
            cde.Wait();

            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }
        private static void Blocking_Collection()
        {
            var rnd = new Random();
            
            //var list = new BlockingCollection<int>(5);
            var list = new BlockingCollection<int>(new ConcurrentStack<int>(), 5);
            //var list = new BlockingCollection<int>(new ConcurrentBag<int>(), 5);
            for (int j = 0; j < 1; j++)
            {
                new Thread((x) =>
                 {
                     for (int i = 0; i < 20; i++)
                     {
                         int nr = 20 * (int)x + i;
                         // Blocks when full
                         list.Add(nr);
                         Console.WriteLine($"Write {nr}");
                     }
                 }).Start(j);
            }

            var t = new Thread(() =>
            {
                while(true) 
                {
                    Thread.Sleep(1000);
                    int nr = list.Take();
                    Console.WriteLine($"Read {nr}");                  
                }
            });
            t.Start();
            t.Join();
        }
        private static void Concurrent_Bag()
        {
            const int PARTICIPANTS = 4;
            var rnd = new Random();
            var cde = new CountdownEvent(PARTICIPANTS);
            var bar = new Barrier(PARTICIPANTS);
            // Not a substitution for List<>
            // ConcurrentBag maintains a collection (stack) per thread.
            // TryTake will take the last value en removes it from the stack
            // When TryTake cannot find a value, it takes a value from another thread.
            // (You might see this happen)
            var list = new ConcurrentBag<int>();

            for(int j=0; j < PARTICIPANTS; j++)
            {
                new Thread((jj) =>
                {
                    bar.SignalAndWait();
                    for (int i = 0; i < 10; i++)
                    {
                        list.TryPeek(out int nr);
                        // Decreasing this value increase the chance of "stealing" a value
                        //Thread.Sleep(1);
                        list.Add(++nr);
                    }
                    cde.Signal();
                }).Start(j) ;
            }

            cde.Wait();
            // This this thread doesn't have a internal stack so it will "steal" a value
            // from another thread in bag. From which one? Random!
            list.TryTake(out int nro);
            Console.WriteLine($"Stolen value:{nro}");

            foreach (var item in list.ToArray())
            {
                Console.WriteLine(item);
            }
            Console.WriteLine($"Numbers in bag: {list.Count}");

            while (list.TryTake(out nro))
            {
                Console.Write($"{nro}, ");
            }
            Console.WriteLine();
            Console.WriteLine($"Numbers in bag: {list.Count}");
        }
        private static void Concurrent_Stack()
        {
            const int PARTICIPANTS = 2;

            // You might have to run the code a few time to see results
            for (int x = 0; x < 200; x++)
                RunSample();
            Console.WriteLine("Ready");

            void RunSample()
            {
                var bar = new Barrier(PARTICIPANTS);
                var cde = new CountdownEvent(PARTICIPANTS);

                var stack = new Stack<int>(Enumerable.Range(1, 100));
                //var stack = new ConcurrentStack<int>(Enumerable.Range(1, 100));

                int total = 0;
                for (int ti = 0; ti < PARTICIPANTS; ti++)
                {
                    new Thread(_ =>
                    {
                        bar.SignalAndWait();
                        while (stack.Count > 0)
                        {
                            int nr = stack.Pop();
                            //stack.TryPop(out int nr);
                            Interlocked.Add(ref total, nr);
                        }
                        cde.Signal();
                    }).Start();
                }

                cde.Wait();
                if (total != 5050)
                    Console.WriteLine($"Sum: {total}");
            }
        }     
    }
}
