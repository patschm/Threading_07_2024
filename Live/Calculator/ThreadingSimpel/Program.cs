
using System;
using System.Globalization;
using System.Security.Principal;
using System.Threading;

namespace ThreadingSimpel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Synchrone();
            //ThreadedVersion();
            //AdvancedThread();
            // DeThreadPool();
            ApmMethod();
            Console.WriteLine("Eind Programma");
            Console.ReadLine();
        }

        private static void ApmMethod()
        {
            Func<int, int, int> fn = LongAdd;
           var ar = fn.BeginInvoke(3, 4, TheCallback, fn);
           
            while(!ar.IsCompleted)
            {
                Console.Write("░");
                Thread.Sleep(100);
            }
            Console.WriteLine();

            //int res = fn.EndInvoke(ar);
            //Console.WriteLine(res);
        }

        static void TheCallback(IAsyncResult ar)
        {
            var fn = ar.AsyncState as Func<int, int, int>;
            int res = fn.EndInvoke(ar);
            Console.WriteLine();
            Console.WriteLine(res);
        }

        private static void DeThreadPool()
        {
            ThreadPool.GetMaxThreads(out int wt, out int cp);
            Console.WriteLine($"{wt} {cp}");

            ThreadPool.GetAvailableThreads(out int nr, out int compl);
            Console.WriteLine($"{nr} completion {compl}");
        }

        private static void AdvancedThread()
        {
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("Jan"), new string[] { "Admin" });

            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
            Thread t = new Thread(() => {
                //Thread.BeginThreadAffinity
                Console.WriteLine(Thread.CurrentPrincipal.Identity.Name);
                int res = LongAdd(2, 3);
                Console.WriteLine(res);
            });
            t.IsBackground = false;
            t.Priority = ThreadPriority.Normal;
            //t.Suspend();
            //t.Resume();
            
            t.Start();
            t.Join(30);
            Console.WriteLine("En verder...");
            Console.WriteLine(t.CurrentCulture);
            Console.WriteLine(t.CurrentUICulture);

            Thread.SpinWait(2000);
           ;
        }

        private static void ThreadedVersion()
        {
            Thread t = new Thread(() => {
                int res = LongAdd(2, 3);
                Console.WriteLine(res);
            });
            t.IsBackground = false;
            t.Start();
        }

        private static void Synchrone()
        {
            int res = LongAdd(2,3);
            Console.WriteLine(res);
        }

        static int LongAdd(int a, int b)
    {
        Thread.Sleep(5000);
        return a + b;
    }
}
}
