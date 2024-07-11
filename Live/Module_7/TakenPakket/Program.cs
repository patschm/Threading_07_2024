
using System.Threading.Channels;

namespace TakenPakket;

internal class Program
{
    static void Main(string[] args)
    {
        //SimpleTask();
        //SimpleTaskFraaier();
        //ErrorTask();
        //Cancellen();
        //Moderner();
        //ExceptionAgainAsync();
        ZakLampTasksAsync();
        //TaskCompletionSource
        Console.WriteLine("En verder");
        Console.ReadLine();
    }

    private static async Task ZakLampTasksAsync()
    {
        var t1 = Task.Delay(2000);
        var t2 = Task.Delay(3000);

        await Task.WhenAll(t1, t2);
        Console.WriteLine("En sub verder...");
    }

    private static async Task ExceptionAgainAsync()
    {
        try
        {
            await Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                throw new Exception("Ooops");
            });
        }
        catch (Exception ex)
        {
            Console.Out.WriteLine(ex.Message);
        }
    }

    private static async void Moderner()
    {
        var t1 = Task.Run(() => LongAdd(7, 8));
        
        Console.WriteLine($"Thread ID = {Thread.CurrentThread.ManagedThreadId}");
        int result = await t1;
        Console.WriteLine($"Thread ID = {Thread.CurrentThread.ManagedThreadId}");
        Console.WriteLine(result);
        result = await LongAddAsync(8, 9);
        Console.WriteLine($"Thread ID = {Thread.CurrentThread.ManagedThreadId}");
        Console.WriteLine(result);
        result = await LongAddAsync(18, 9);
        Console.WriteLine($"Thread ID = {Thread.CurrentThread.ManagedThreadId}");
        Console.WriteLine(result);

        //   .ContinueWith(pt => Console.WriteLine(pt.Result));
    }

    private static void Cancellen()
    {
        CancellationTokenSource nikko = new CancellationTokenSource();

        CancellationToken bommetje =nikko.Token;
        Task.Run(() =>
        {
            for (; ; )
            {
                //try
                //{
                //    bommetje.ThrowIfCancellationRequested();
                //}
                //catch {

                //    throw new Exception("Ooops");
                //}
                if (bommetje.IsCancellationRequested)
                {
                    Console.WriteLine("Kabooom");
                    return;
                }
                Task.Delay(1000).Wait();
                Console.WriteLine($"Ping");
            }
        }).ContinueWith(pt=>Console.WriteLine(pt.Status));

        nikko.CancelAfter(5000);
    }

    private static void ErrorTask()
    {
        //AppDomain.CurrentDomain.UnhandledException += (s, arg) => { Console.WriteLine("Error"); };
        //try
        //{
        //    Task.Run(() =>
        //    {
        //        Task.Delay(1000).Wait();
        //        throw new Exception("Ooops");
        //    });
        //}
        //catch(Exception e)
        //{
        //    Console.WriteLine(e.Message);
        //}
        Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                throw new Exception("Ooops");
            }).ContinueWith(pt =>
            {
                if (pt.Exception != null)
                {
                    Console.WriteLine(pt.Exception?.InnerException?.Message);
                }
                Console.WriteLine(pt.Status);
            });
    }

    private static void SimpleTask()
    {
        Task<int> t = new Task<int>(() => {
            int res = LongAdd(4, 5);
            return res;
        });

        t.Start();

        Console.WriteLine(t.Result);
    }
    private static void SimpleTaskFraaier()
    {
        Task<int> tttt = new Task<int>(() =>
        {
            int res = LongAdd(4, 5);
            return res;
        });
        tttt.ContinueWith(prevTask => {
            Console.WriteLine(prevTask.Result);
        });
        tttt.ContinueWith(prevTask => { Console.WriteLine(prevTask.Status); return 42; })
            .ContinueWith(pt => Console.WriteLine(pt.Result));


        tttt.Start();
        
        //tttt.Start();

        Task.Run(()=>LongAdd(7,8))
            .ContinueWith(pt=>Console.WriteLine(pt.Result));
    }

    static int LongAdd(int a, int b)
    {
        Task.Delay(5000).Wait();
        return a + b;
    }
    static Task<int> LongAddAsync(int a, int b)
    {
        return Task.Run(() => LongAdd(a, b));
    }
}
