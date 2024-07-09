namespace M11_DebugExceptions;

// Snapshot Debugging (Tools->Options->IntelliTrace->IntelliTrace Snapshots)
internal class Program
{
    static async Task Main(string[] args)
    {
        await TaskA();
        Console.ReadLine();
    }
    static async Task<int> TaskA()
    {
        return await TaskB();
    }

    static async Task<int> TaskB()
    {
        await Parallel.ForAsync(0, 10, async (idx, token) =>
        {
            await TaskC(idx);
        });
       return 0;
    }

    static async Task<int> TaskC(int x)
    {
        int low = rnd.Next(0, 4);
        return await Task.FromResult(x/low);
    }

    static Random rnd = new Random();
}
