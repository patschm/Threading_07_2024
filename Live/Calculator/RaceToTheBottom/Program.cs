using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RaceToTheBottom
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //LockingMeuk();
            //OverProcessBoundaries();

            //Rekenen();
            MoreLocking();


            Console.ReadLine();
        }

        private static void MoreLocking()
        {
            Barrier bar = new Barrier(10);
            CountdownEvent cde = new CountdownEvent(10);

            int a = 0;
            for (int i = 0;i < 10; i++)
            {
                new Thread(() => { 
                bar.SignalAndWait();
                    Thread.Sleep(10);
                    //a = a + 1;
                    Interlocked.Increment(ref a);
                    int val = Interlocked.CompareExchange(ref a, 100, 6);
                    Console.WriteLine($"Val is {val} a= {a}");
                   // Console.WriteLine(a);
                    cde.Signal();
                }).Start();
            }
            cde.Wait();
        }

        private static void Rekenen()
        {
            int a = 0;
            int b = 0;

            //ManualResetEventSlim
            using (AutoResetEvent zl1 = new AutoResetEvent(false))
            using (AutoResetEvent zl2 = new AutoResetEvent(false))
            {

                new Thread(() =>
                {
                    Thread.Sleep(2000);
                    a = 10;
                    zl1.Set();
                }).Start();

                new Thread(() =>
                {
                    Thread.Sleep(3000);
                    b = 20;
                    zl2.Set();
                }).Start();


                WaitHandle.WaitAll(new WaitHandle[] { zl1, zl2 });
                int result = a + b;
                Console.WriteLine(result);

                //zl1.Reset();
                //zl2.Reset();
                new Thread(() =>
                {
                    Thread.Sleep(2000);
                    a = 90;
                    zl1.Set();
                }).Start();

                new Thread(() =>
                {
                    Thread.Sleep(3000);
                    b = 50;
                    zl2.Set();
                }).Start();


                WaitHandle.WaitAll(new WaitHandle[] { zl1, zl2 });
                result = a + b;
                Console.WriteLine(result);
            }
        }

        private static void OverProcessBoundaries()
        {
            Console.WriteLine("We Beginnen...");
            Mutex mx = new Mutex(false, "Iets Unieks");

            mx.WaitOne();
            Console.WriteLine("Bezig.... Press enter to continue");
            Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);

            Console.ReadLine();
            mx.ReleaseMutex();
            Console.WriteLine("Eind");
        }

        private static void LockingMeuk()
        {
            var worker = new Worker();
            Barrier bar = new Barrier(10);
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    bar.SignalAndWait();
                    //Worker.DoJob(i);
                    worker.DoJob(i);
                }, i);
                //Worker.DoJob(i);
                //worker.DoJob(i);    
            }
        }
    }

   //[Synchronization]
    public class Worker //: ContextBoundObject
    {
        int a = 3, b = 1;
        static object talkingStick = new object();

        public void DoJob(object o)
        {
            //try
            //{
            //    //bool take = false;
            //    Monitor.Enter(talkingStick);
            //    if (b != 0)
            //    {
            //        Thread.Sleep(10);
            //        int res = a / b;
            //        b = 0;
            //        Console.WriteLine(res);
            //    }
            //}
            //finally
            //{
            //    Monitor.Exit(talkingStick);
            //}
           
            lock (talkingStick)
            {
                if (b != 0)
                {
                    Thread.Sleep(10);
                    int res = a / b;
                    b = 0;
                    Console.WriteLine(res);
                }
            }
        }
    }
}
