using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Threading;

namespace M2_Locking
{
    internal class Program
    {
        static bool running = false;
       
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == nameof(running)) running = true;
            ExclusiveLocking_1();
            //ExclusiveLocking_2();
            //ExclusiveLocking_3();
            //ExclusiveLocking_4();
            //ExclusiveLocking_5();
            Console.ReadLine();
        }

        // Exclusive locking. Only one thread can execute a piece of code or section
        // A Critical Section
        private static void ExclusiveLocking_1()
        {
            for (int i = 0; i < 10; i++)
            {
                //new Thread(Worker.DoJob).Start();
                new Thread(Worker.DoJobSafeBasic).Start();
            }
        }
        private static void ExclusiveLocking_2()
        {
            // While Monitor or lock works only within the process, Mutex works across process boundaries
            // Whether or not a locking mechanism works across process bounderies depends on whether
            // the mechanisme derives from MarshalByRefObject (class used in ancient .NET Remoting)
            Mutex m1 = new Mutex(initiallyOwned: false, "A_UNIQUE_NAME");
            Console.WriteLine("Waiting for the mutex");
            m1.WaitOne();
            Console.WriteLine("We entered the mutex. Try start a new instance of this program");
            if (!running)
                Process.Start(Assembly.GetExecutingAssembly().Location, nameof(running));
            Console.WriteLine("Press Enter to exit the mutex.");
            Console.ReadLine();
            m1.ReleaseMutex();
            Console.WriteLine("Exit the mutex");
        }
        private static void ExclusiveLocking_3()
        {
            // WorkerSync is a synchronous class (See definition)
            WorkerSync worker = new WorkerSync();
            for (int i = 0; i < 10; i++)
            {
                new Thread(worker.DoJob).Start();
            }
        }
        private static void ExclusiveLocking_4()
        {
            List<int> list = new List<int>();
            var rnd = new Random();
            for (int i = 0; i < 10; i++) 
            { 
                new Thread(() => {
                    for (int j = 0; j < 10; j++)
                    {
                        int nr = rnd.Next(1, 20);
                        lock (list)
                        {
                            if (!list.Contains(nr))
                            {
                                list.Add(nr);
                            }
                        }
                    }
                    lock (list)
                    {
                        var nrs = list.ToArray();
                        Console.WriteLine(String.Join(", ", nrs));
                    }
                }).Start();
            }

        }
        private static void ExclusiveLocking_5()
        {
            var barrier = new Barrier(10);
            int counter = 0;
            for(int i = 0;i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem((idx) => {
                    barrier.SignalAndWait();
                    for(int j = 0; j < 10; j++)
                    {
                        int tmp = counter;
                        Thread.Sleep(10);
                        counter = ++tmp;
                        //bool equal  = Interlocked.Equals(counter, tmp);
                        //Console.WriteLine(equal);
                        //Interlocked.Increment(ref counter);
                        
                    }
                    Console.WriteLine(counter);
                }, i);
            }           
        }

    }

    public class Worker
    {
        static int a, b = 1;

        public static void DoJob()
        {
            if (b != 0)
            {
                Thread.Sleep(10);
                Console.WriteLine(a / b);
            }
            b = 0;
        }

        static object locker = new object();
        public static void DoJobSafe()
        {
            lock (locker)
            {
                if (b != 0)
                {
                    Thread.Sleep(10);
                    Console.WriteLine(a / b);
                }
                b = 0;
            }
        }
        public static void DoJobSafeBasic()
        {
            bool hasLock = false;
            try
            {
                // Monitor is the mechanism behind lock().
                // hasLock might be overkill. 
                // It's for the unlikely event that your program crashes
                // between try and Monitor.Enter (OutOfMemory)
                // You probably can do without.
                Monitor.Enter(locker, ref hasLock);

                if (b != 0)
                {
                    Thread.Sleep(10);
                    Console.WriteLine(a / b);
                }
                b = 0;
            }
            finally
            {
                if (hasLock)
                    Monitor.Exit(locker);
            }
        }
    }

    
    [Synchronization]
    public class WorkerSync : ContextBoundObject
    {
        static int a, b = 1;

        // This class is synchronized so only one thread can access members of this class.
        public void DoJob()
        {
            if (b != 0)
            {
                Thread.Sleep(10);
                Console.WriteLine(a / b);
            }
            b = 0;
        }
    }
}
