using System.Diagnostics;

namespace M4b_AsyncResult;

internal class Program
{
    static void Main(string[] args)
    {
        //Yielding();
        YieldingAsync();
        // Exercise 4
        Console.ReadLine();
    }

    private static void Yielding()
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();
        foreach (int nr in  Numbers())
        {
           Console.WriteLine(nr);
        }
        watch.Stop();
        Console.WriteLine(watch.Elapsed );
    }
    private static async Task YieldingAsync()
    {
        await foreach (int nr in NumbersAsync())
        {
            Console.WriteLine(nr);
        }
    }
    private static IEnumerable<int> Numbers()
    {
        //[] buffer = new int[1_000_000_000];
        //for(int i =0; i <1_000_000_000;  i++)
        //{
        //    yield return i;
        //    //buffer[i] = i;
        //}
        ////return buffer;
        ////int[] ints = new int[3];
        Console.WriteLine("Next 1");
        yield return 1;
        Console.WriteLine("Next 2");
        yield return 2;
        Console.WriteLine("Next 3");
        yield return 3;
    }
    // private static async Task<IEnumerable<int>> NumbersAsync()
    private static async IAsyncEnumerable<int> NumbersAsync()
    {
        Console.WriteLine("Next 1");
        yield return await Task.FromResult(1);
        Console.WriteLine("Next 2");
        yield return await Task.FromResult(2);
        Console.WriteLine("Next 3");
        yield return await Task.FromResult(3);
    }
}
