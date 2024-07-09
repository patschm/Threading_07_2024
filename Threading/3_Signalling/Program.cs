using System;
using System.Threading;
using System.Threading.Tasks;

namespace M3_Signalling
{
    internal class Program
    {
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            BasicSignalling();
            //CountDownApproval();
            // Exercise 2

            //BarrierSprint();
            //SemaphoreGarage();
            //ReadersAndWriters();

            // Exercise 3a
            Console.ReadLine();
        }

        private static void BasicSignalling()
        {
            //var flashlight1 = new ManualResetEvent(false);
            //var flashlight2 = new ManualResetEvent(false);
            var flashlight1 = new AutoResetEvent(false);
            var flashlight2 = new AutoResetEvent(false);

            new Thread(() => {
                Thread.Sleep(1000);
                flashlight1.Set();
                Thread.Sleep(6000);
                flashlight1.Set();
            }).Start();
            new Thread(() => {
                Thread.Sleep(2000);
                flashlight2.Set();
                Thread.Sleep(5000);
                flashlight2.Set();
            }).Start();

            WaitHandle.WaitAll(new WaitHandle[] { flashlight1, flashlight2 });
            Console.WriteLine("Seen both");
            WaitHandle.WaitAll(new WaitHandle[] { flashlight1, flashlight2 });
            Console.WriteLine("Seen both again");
        }

        private static void ReadersAndWriters()
        {
            var rnd = new Random();
            var rwl = new ReaderWriterLock();

            for(var i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(Reader, i);
            }

            void Reader(object nr)
            {
                for (var j = 0; j < 30; j++)
                {
                    var useTime = rnd.Next(1000, 5000);
                    rwl.AcquireReaderLock(Timeout.Infinite);
                    Console.WriteLine($"Client {nr} is reading...");
                    Thread.Sleep(useTime);
                    if (useTime > 4000)
                    {
                        var cookie = rwl.UpgradeToWriterLock(Timeout.Infinite);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Client {nr} is wants to write...");
                        Thread.Sleep(rnd.Next(5000, 10000));
                        Console.WriteLine($"Client {nr} finished writing");
                        Console.ResetColor();
                        rwl.DowngradeFromWriterLock(ref cookie);
                    }
                    Console.WriteLine($"Client {nr} finished reading");
                    rwl.ReleaseReaderLock();
                }
            }
            void Writer(object nr)
            {
                rwl.AcquireWriterLock(Timeout.Infinite);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Client {nr} is wants to write...");
                Thread.Sleep(rnd.Next(5000, 10000));
                Console.WriteLine($"Client {nr} finished writing");
                Console.ResetColor();
                rwl.ReleaseWriterLock();
            }
        }

        static object locker = new object();
        private static void SemaphoreGarage()
        {
            var rnd = new Random();
            Semaphore trafficLight = new Semaphore(25, 25);

            var max = 0;
            for (var i = 0; i < 100; i++) 
            {
                ThreadPool.QueueUserWorkItem(Car, i);       
            }

            void Car(object nr)
            {
                if (max >= 25)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Car {nr} arriving parking lot...");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Car {nr} arriving parking lot...");
                    Console.ResetColor();
                }
                trafficLight.WaitOne();
                lock (locker)
                {
                    max++;
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"\tCar {nr} driving into the parking lot ({25 - max} spaces left)");
                    Console.ResetColor();
                }
                var delay = rnd.Next(1000, 10000);
                Thread.Sleep(20000 + delay);
                Console.WriteLine($"Car {nr} driving out...");
                trafficLight.Release();
                lock(locker) max--;
            }
        }

        private static void CountDownApproval()
        {
            CountdownEvent approvers = new CountdownEvent(10); // We need ten approvals to continue;

            Console.WriteLine("After writing code...");
            Console.WriteLine("After commit code...");
            Console.WriteLine("Create Pull request...");
            for (int i = 0;i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(idx => {
                    Console.WriteLine($"Waiting for approver {idx}...");
                    Thread.Sleep(3000 + rnd.Next(1000, 2000));
                    Console.WriteLine($"Approver {idx} approved");
                    approvers.Signal();
                }, i);
            }
            approvers.Wait();
            Console.WriteLine("All approvers responded. Now the merge is complete");


        }

        private static void BarrierSprint()
        {
            Barrier runners = new Barrier(8);
            ManualResetEvent starter = new ManualResetEvent(false);
            //runners.AddParticipants(8);
            Console.WriteLine("On your marks");
            for (int i = 1; i <= 8; i++)
            {
                ThreadPool.QueueUserWorkItem((number) =>
                {
                    Console.WriteLine($"Runner {number} positions into the starting blocks");
                    Thread.Sleep(2000 + rnd.Next(0, 3000));
                    Console.WriteLine($"Runner {number} is positioned");
                    runners.SignalAndWait();
                    //starter.WaitOne(); // Only for the show. To display starter message only once.
                    Console.WriteLine($"Runner {number} preparing to start");
                    Thread.Sleep(1000 + rnd.Next(0, 1000));
                    Console.WriteLine($"Runner {number} is ready for start");
                    runners.SignalAndWait();
                   // starter.WaitOne();
                    Console.WriteLine($"Runner {number} is running as fast as (s)he can");
                }, i);
            }
            //Thread.Sleep(6000);
            //Console.WriteLine("Get set!");
            //starter.Set();
            
            //starter.Reset();
            //Thread.Sleep(2200);  
            //Console.WriteLine("Go!");
            //starter.Set();
        }

        private static void Runner()
        {
            Thread.Sleep(1000);
        }
    }
}
