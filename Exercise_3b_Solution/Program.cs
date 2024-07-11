using System.Collections.Concurrent;

namespace Problem_Shared_Stack
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConcurrentStack<int> buffer = new ConcurrentStack<int>();
            NumberReader reader = new NumberReader(buffer);
            const int NR_READERS = 1;
            const int NR_WRITERS = 2;

            for (int i = 0; i < NR_WRITERS; i++)
                new NumberWriter(buffer).Run();

            for (int i = 1; i < NR_READERS; i++)
                new NumberReader(buffer).Run();

            var t2 = reader.Run();
            t2.Join();

            Console.WriteLine("End Program");
            Console.ReadLine();
        }
    }
}
