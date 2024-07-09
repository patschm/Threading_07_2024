using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Security.Principal;

namespace M1_Basics;

internal class Program
{
    static void Main(string[] args)
    {
        Simple();
        //Control();
        //Pausing();
        //Joining();
        //Sleeping();
        //SpinWaitSleep();
        //BackgroundWorker();
        //ThreadPooling();
        //ThreadProperties();

        Console.WriteLine("End Program");
        Console.ReadLine();
    }

    private static void Simple()
    {
        Thread t1 = new Thread(Work);
        t1.Start();
        for (int i = 0; i < 1000; i++)
            Console.Write($"tm{i,-4}");
        Console.WriteLine();

        
    }
    private static void Control()
    {
        Thread t1 = new Thread(Work);
        t1.IsBackground = false;  // Remove Console.ReadLine() at the end of Main()
        t1.Priority = ThreadPriority.Lowest;
        t1.Priority = ThreadPriority.BelowNormal;
        t1.Priority = ThreadPriority.Normal; // Default
        t1.Priority = ThreadPriority.AboveNormal;
        t1.Priority = ThreadPriority.Highest;
        t1.Start();
    }
    private static void Pausing()
    {
        Thread t1 = new Thread(Work);
        t1.Start();
        for (int i = 0; i < 1000; i++)
        {
            if (i == 200) t1.Suspend();
            if (i == 800) t1.Resume();
            Console.Write($"tm{i,-4}");
        }
        t1.Abort("Kappen");
    }
    private static void Joining()
    {
        Thread t1 = new Thread(Work);
        t1.Start();
        t1.Join();
        for (int i = 0; i < 1000; i++)
            Console.Write($"tm{i,-4}");
        Console.WriteLine();
    }
    private static void Sleeping()
    {
        Thread t1 = new Thread(Work);
        Console.WriteLine(t1.ThreadState);
        t1.Start();
        Console.WriteLine(t1.ThreadState);

        for (int i = 0; i < 1000; i++)
        {
            if (i == 200) Thread.Sleep(1000);
            Console.Write($"tm{i,-4}");
        }
        Console.WriteLine();
        Thread t2 = new Thread(() =>
        {
            try
            {
                Thread.Sleep(10000);
            }
            catch (ThreadInterruptedException)
            {
                // Caught when interrupt is called
                Console.WriteLine("...?");
            }
        });
        t2.Start();

        Thread.Sleep(500);
        Console.WriteLine(t2.ThreadState);
        t2.Interrupt(); // Stops a thread that is in sleeping state
        Console.WriteLine(t2.ThreadState);
        Thread.Sleep(100);
        // Will be stopped if interrupt exception is caught. Otherwise running.
        Console.WriteLine(t2.ThreadState);
    }
    private static void SpinWaitSleep()
    {
        Console.WriteLine("Sleep Test");
        Thread t1 = new Thread(() =>
        {
            Console.WriteLine("T1 start");
            Thread.Sleep(1000);
            Console.WriteLine("T1 finished");
        });
        t1.Start();
        Console.WriteLine("End T1 section");
        t1.Join();
        Console.WriteLine("SpinWait Test");
        Thread t2 = new Thread(() =>
        {
            Console.WriteLine("T2 start");
            Thread.SpinWait(10000);
            Console.WriteLine("T2 finished");
        });
        t2.Start();
        Console.WriteLine("End T2 section");
        t2.Join();
    }
    // Exercise 1a
    private static void BackgroundWorker()
    {
        BackgroundWorker worker = new BackgroundWorker();
        worker.WorkerReportsProgress = true;
        worker.WorkerSupportsCancellation = true;
        worker.DoWork += (s, a) =>
        { 
            var sender = s as BackgroundWorker;
            int i = 0;
            for (; i < 42; i++)
            {
               
                sender?.ReportProgress((100/42) * i);
                Thread.Sleep(200);
            }
            sender?.ReportProgress(100);
            a.Result = i;
        };
        worker.RunWorkerCompleted += (s, a) =>
        {
            Console.WriteLine();
            Console.WriteLine($"Answer: {a.Result}");
        };
        worker.ProgressChanged += (s, a) => {
            Console.Clear();
            Console.WriteLine($"Percentage complete: {a.ProgressPercentage}%");
        };
        
        worker.RunWorkerAsync();
    }
    // Exercise 1b
    private static void ThreadPooling()
    {
        Stopwatch watch = new Stopwatch();
        Console.WriteLine("Without pooling");
        watch.Start();
        for (int i = 0; i < 10; i++)
        {
            Thread t = new Thread(WorkToo);
            t.Start();
        }
        watch.Stop();
        Console.WriteLine($"Took {watch.Elapsed}s to initialize");
        Console.WriteLine("Now with pooling");
        watch.Reset();
        watch.Start();
        for (int i = 0; i < 10; i++)
        {
            ThreadPool.QueueUserWorkItem(WorkToo, i);
        }
        watch.Stop();
        Console.WriteLine($"Took {watch.Elapsed}s to initialize");
    }
    private static void ThreadProperties()
    {
        AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
        ShowThreadInfo();

        // Change Settings
        Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
        Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("Pierre"), ["admin"]);
        ShowThreadInfo();

        // New Thread
        Thread t = new Thread(() =>
        {
            ShowThreadInfo();
        });
        t.Start();

        void ShowThreadInfo()
        {
            Console.WriteLine("Thread Info");
            Console.WriteLine($"Thread ID {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Culture Info: {Thread.CurrentThread.CurrentCulture}");
            Console.WriteLine($"User: {Thread.CurrentPrincipal?.Identity?.Name}");
            Console.WriteLine("-------------------------------------------------------------------");
        }
    }
    // Exercise 1c

    static void Work(object? obj)
    {
        if (obj != null)
            Console.WriteLine($"Running {obj}th thread");
        for (int i = 0; i < 1000; i++)
            Console.Write($"ts{i,-4}");
        Console.WriteLine();
    }
    static void WorkToo(object? obj)
    {
        if (obj == null) obj = 1;
        Console.WriteLine($"Running {obj}th thread");
        // Console.WriteLine("Do work");
    }
}

