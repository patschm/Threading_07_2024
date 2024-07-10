using System.Diagnostics;
using System.Numerics;

class Program
{
    static Task? t1, t2, t3, t4;
    static int aa = 0;
    static int bb = 0;
    static int cc = 0;
    static bool waitFor1 = true;
    static bool waitFor5 = true;
    static int pcount;
    static object mylock = new object();

    static void Main()
    {
        Simple();
        //ComPlex();
        
        Console.ReadLine();
    }

    private static void Simple()
    {
        for(int i = 0; i < 4; i++)
        {
            Task.Run(() =>
            {
                Task.Delay(2000).Wait();
                int z = 10 * 42;
                Console.WriteLine(z);
            });
        }
    }
    private static void ComPlex()
    {
        pcount = Environment.ProcessorCount;
        Console.WriteLine("Proc count = " + pcount);
        ThreadPool.SetMinThreads(4, -1);
        ThreadPool.SetMaxThreads(4, -1);

        t1 = new Task(TaskA, 1);
        t2 = new Task(TaskA, 2);
        t3 = new Task(TaskA, 3);
        t4 = new Task(TaskA, 4);
        Console.WriteLine("Starting t1 " + t1.Id.ToString());
        t1.Start();
        Console.WriteLine("Starting t2 " + t2.Id.ToString());
        t2.Start();
        Console.WriteLine("Starting t3 " + t3.Id.ToString());
        t3.Start();
        Console.WriteLine("Starting t4 " + t4.Id.ToString());
        t4.Start();

    }
    
    static void TaskA(object? o)
    {
        TaskB(o);
    }
    static void TaskB(object? o)
    {
        TaskC(o);
    }
    static void TaskC(object? o)
    {
        int temp = (int)o;

        Interlocked.Increment(ref aa);
        while (aa < 4)
        {
            ;
        }

        if (temp == 1)
        {
            // BP1 - all tasks in C
            Debugger.Break();
            waitFor1 = false;
        }
        else
        {
            while (waitFor1)
            {
                ;
            }
        }
        switch (temp)
        {
            case 1:
                TaskD(o);
                break;
            case 2:
                TaskF(o);
                break;
            case 3:
            case 4:
                TaskI(o);
                break;
            default:
                Debug.Assert(false, "fool");
                break;
        }
    }
    static void TaskD(object o)
    {
        TaskE(o);
    }
    static void TaskE(object o)
    {
        // break here at the same time as H and K
        while (bb < 2)
        {
            ;
        }
        //BP2 - 1 in E, 2 in H, 3 in J, 4 in K
        Debugger.Break();
        Interlocked.Increment(ref bb);

        //after
        TaskL(o);
    }
    static void TaskF(object o)
    {
        TaskG(o);
    }
    static void TaskG(object o)
    {
        TaskH(o);
    }
    static void TaskH(object o)
    {
        // break here at the same time as E and K
        Interlocked.Increment(ref bb);
        Monitor.Enter(mylock);
        while (bb < 3)
        {
            ;
        }
        Monitor.Exit(mylock);


        //after
        TaskL(o);
    }
    static void TaskI(object o)
    {
        TaskJ(o);
    }
    static void TaskJ(object o)
    {
        int temp2 = (int)o;

        switch (temp2)
        {
            case 3:
                t4?.Wait();
                break;
            case 4:
                TaskK(o);
                break;
            default:
                Debug.Assert(false, "fool2");
                break;
        }
    }
    static void TaskK(object o)
    {
        // break here at the same time as E and H
        Interlocked.Increment(ref bb);
        Monitor.Enter(mylock);
        while (bb < 3)
        {
            ;
        }
        Monitor.Exit(mylock);


        //after
        TaskL(o);
    }
    static void TaskL(object oo)
    {
        int temp3 = (int)oo;

        switch (temp3)
        {
            case 1:
                TaskM(oo);
                break;
            case 2:
                TaskN(oo);
                break;
            case 4:
                TaskO(oo);
                break;
            default:
                Debug.Assert(false, "fool3");
                break;
        }
    }
    static void TaskM(object o)
    {
        // breaks here at the same time as N and Q
        Interlocked.Increment(ref cc);
        while (cc < 3)
        {
            ;
        }
        //BP3 - 1 in M, 2 in N, 3 still in J, 4 in O, 5 in Q
        Debugger.Break();
        Interlocked.Increment(ref cc);
        while (true)
            Thread.Sleep(500); // for ever
    }
    static void TaskN(object o)
    {
        // breaks here at the same time as M and Q
        Interlocked.Increment(ref cc);
        while (cc < 4)
        {
            ;
        }
        TaskR(o);
    }
    static void TaskO(object o)
    {
        Task t5 = Task.Factory.StartNew(TaskP, TaskCreationOptions.AttachedToParent);
        t5.Wait();
        TaskR(o);
    }
    static void TaskP()
    {
        Console.WriteLine("t5 runs " + Task.CurrentId.ToString());
        TaskQ();
    }
    static void TaskQ()
    {
        // breaks here at the same time as N and M
        Interlocked.Increment(ref cc);
        while (cc < 4)
        {
            ;
        }
        // task 5 dies here freeing task 4 (its parent)
        Console.WriteLine("t5 dies " + Task.CurrentId.ToString());
        waitFor5 = false;
    }
    static void TaskR(object o)
    {
        if ((int)o == 2)
        {
            //wait for task5 to die
            while (waitFor5) {; }


            int i;
            //spin up all procs
            for (i = 0; i < pcount - 4; i++)
            {
                Task t = Task.Factory.StartNew(() => { while (true) ; });
                Console.WriteLine("Started task " + t.Id.ToString());
            }

            Task.Factory.StartNew(TaskT, i + 1 + 5, TaskCreationOptions.AttachedToParent); //scheduled
            Task.Factory.StartNew(TaskT, i + 2 + 5, TaskCreationOptions.AttachedToParent); //scheduled
            Task.Factory.StartNew(TaskT, i + 3 + 5, TaskCreationOptions.AttachedToParent); //scheduled
            Task.Factory.StartNew(TaskT, i + 4 + 5, TaskCreationOptions.AttachedToParent); //scheduled
            Task.Factory.StartNew(TaskT, (i + 5 + 5).ToString(), TaskCreationOptions.AttachedToParent); //scheduled

            //BP4 - 1 in M, 2 in R, 3 in J, 4 in R, 5 died
            Debugger.Break();
        }
        else
        {
            Debug.Assert((int)o == 4);
            t3?.Wait();
        }
    }
    static void TaskT(object? o)
    {
        Console.WriteLine("Scheduled run " + Task.CurrentId.ToString());
    }
}