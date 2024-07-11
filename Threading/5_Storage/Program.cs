using System.Threading.Tasks;
using System.Threading;
using System;

namespace M5_Storage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //ValuePerThread();
            //ValuePerThread2();
            //ValuePerThread3();
            //ValuePerThread4Async();
            LazyInit();
            Console.ReadLine();
        }


        // Every thread gets it's own dedicated version of this variable
        [ThreadStatic]
        static int counter = 0;
        private static void ValuePerThread()
        {
            Barrier bar = new Barrier(10);

            var options = new ParallelOptions { MaxDegreeOfParallelism = 14 };

            Parallel.For(0, 10, options, idx =>
            {
                for (int i = 0; i < 10; i++)
                {
                    bar.SignalAndWait();
                    counter += 1;
                }
                Console.WriteLine(counter);
            });
            Console.WriteLine(counter);
        }
        private static void ValuePerThread2()
        {
            // Unlike ThreadStatic ThreadLocal works also on instance fields or locals
            ThreadLocal<int> local = new ThreadLocal<int>();
            //int local = 0;
            Barrier bar = new Barrier(10);

            Parallel.For(0, 10, idx =>
            {
                //int local = 0;
                for (int i = 0; i < 10; i++)
                {
                    //bar.SignalAndWait();
                    local.Value++;
                    //local++;
                }
                Console.WriteLine($"ID: {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine(local);
            });
            Console.WriteLine("========");
            //Console.WriteLine(local.Values);
        }
        private static void ValuePerThread3()
        {
            LocalDataStoreSlot slot = Thread.GetNamedDataSlot("data");
            Barrier bar = new Barrier(10);

            Parallel.For(0, 10, idx =>
            {
                for (int i = 0; i <= 10; i++)
                {
                    bar.SignalAndWait();
                    Thread.SetData(slot, i);
                }
                Console.WriteLine(Thread.GetData(slot));
            });
        }
        private static async Task ValuePerThread4Async()
        {
            // ThreadLocal is storage per thread (Each task possibly has a different thread)
            ThreadLocal<string> tlocal = new ThreadLocal<string>();
            // AsyncLocal persist across task flow (like await)
            AsyncLocal<string> alocal = new AsyncLocal<string>();

            tlocal.Value = "Value 1";
            alocal.Value = "Value 1";
            var t1 = SomeMethodAsync();
            tlocal.Value = "Value 2";
            alocal.Value = "Value 2";
            var t2 = SomeMethodAsync();

            t1.Wait();
            t2.Wait();

            async Task SomeMethodAsync()
            {
                Console.WriteLine($"Enter. ThreadLocal: {tlocal.Value}, AsyncLocal: {alocal.Value}");
                await Task.Delay(100);
                Console.WriteLine($"Exit. ThreadLocal: {tlocal.Value}, AsyncLocal: {alocal.Value}");
            }
        }
        private static void LazyInit()
        {
            HeavyLoad heavy = null;
            Lazy<HeavyLoad> heavyLoad = null;

            Barrier bar = new Barrier(10);

            Parallel.For(0, 10, idx =>
            {
                bar.SignalAndWait();
                heavy = new HeavyLoad();
                heavyLoad = new Lazy<HeavyLoad>(() => new HeavyLoad(), true);
            });
            Console.WriteLine($"Normal: {heavy?.Counter}");
            Console.WriteLine($"Lazy: {heavyLoad?.Value?.Counter}");
        }
    }
}
