

using System.ComponentModel;

namespace CoreVersie
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Environment.ProcessorCount);
            //ThreadHacks();
            //ShowNrOfThreads();
            //Thread.Sleep(5000);
            //ShowNrOfThreads();
            //HopelijkGaatHetFout();
            //DitOok();
            BackgroundWorkerTest();
            Console.ReadLine();
        }

        private static void BackgroundWorkerTest()
        {
            //int res = 0;
           BackgroundWorker bgw = new BackgroundWorker();
            bgw.WorkerReportsProgress = true;
            bgw.DoWork += (s, a) =>
            {
                var bg = s as BackgroundWorker;
               for(int i = 0; i < 100; i++) 
                {
                    Thread.Sleep(100);
                    bg.ReportProgress(i);
                }
                a.Result = 42;
            };
            bgw.RunWorkerCompleted += (s, a) =>
            {
                Console.WriteLine(a.Result);
            };
            bgw.ProgressChanged += (s, a) =>
            {
                Console.Write($"{a.ProgressPercentage}, ");
            };


            bgw.RunWorkerAsync();
           
        }

        private static void DitOok()
        {
            for(int i = 0; i < 10; i++) {
                new Thread((a) =>
                {
                    Thread.Sleep(1000);
                    Console.WriteLine($"Nr is {a}");
                }).Start(i);
            }
        }

        private static void HopelijkGaatHetFout()
        {
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(ix =>
                {
                    Thread.Sleep(1000);
                    Console.WriteLine($"Thread started {ix}");      
                }, i);
            }
        }

        private static void ThreadHacks()
        {
            ThreadPool.SetMinThreads(10, 10);
            CountdownEvent cde = new CountdownEvent(10);
            ShowNrOfThreads();
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    //Thread.BeginThreadAffinity()
                    //Thread.CurrentThread.IsBackground = true;
                   Console.WriteLine($"Thread started {Thread.CurrentThread.ManagedThreadId}");
                    cde.Signal();
                    Thread.Sleep(5000);
                });
            }
            cde.Wait();
            ShowNrOfThreads();

            cde.Reset();
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    //Thread.CurrentThread.IsBackground = false;
                    Console.WriteLine($"Thread started {Thread.CurrentThread.ManagedThreadId}");
                    cde.Signal();
                    Thread.Sleep(5000);
                    Console.WriteLine("End");
                });
            }
           // cde.Wait();
            ShowNrOfThreads();
        }

        private static void ShowNrOfThreads()
        {
            Console.WriteLine($"Nr of threads {ThreadPool.ThreadCount}");

        }

        static int LongAdd(int a, int b)
        {
            Thread.Sleep(5000);
            return a + b;
        }
    }
}
