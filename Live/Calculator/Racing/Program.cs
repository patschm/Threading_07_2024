namespace Racing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Barrier bar = new Barrier(10);
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(_ => { 
                    bar.SignalAndWait();
                    Worker.DoJob(i);
                }, i);
                //Worker.DoJob();
            }
            
            Console.ReadLine();
        }
    }

   // [Synchronization]
    public class Worker : ContextBoundObject
    {
        static int a = 0;

        public static void DoJob(object? o)
        {
            int tmp = a;
            Thread.Sleep(100);
            a = ++tmp;
            Console.WriteLine(a );
        }
    }
}
  
