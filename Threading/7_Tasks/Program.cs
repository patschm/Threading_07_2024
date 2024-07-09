
namespace M7_Tasks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //SimpleTask();
            //ContinueTask();
            //CancelTask();
            // ExceptionTask();
            //AwaiterTask();
            //AwaitTask();
            AwaitExceptionTask();
            //LegacyTasks();
            Console.WriteLine("End Program");
            Console.ReadLine();
        }

        private static void SimpleTask()
        {
            var t1 = new Task(() =>
            {
                int result = LongAdd(2, 3);
                Console.WriteLine($"Answer: {result}");
            });
            t1.Start();

            Task.Run(() =>
            {
                int result = LongAdd(3, 4);
                Console.WriteLine($"Answer: {result}");
            });

            // Not supported on .NET platform
            //Func<int, int, int> fn = LongAdd;
            //var t3=Task.Factory.FromAsync(fn.BeginInvoke(5,6,null, null), fn.EndInvoke);
            //Console.WriteLine($"Answer: {t3.Result}");

            var t2 = Task.Run(() => LongAdd(4, 5));
            Console.WriteLine($"Answer: {t2.Result}");
        }
        private static void ContinueTask()
        {
            Task.Run(() => LongAdd(1, 2))
                .ContinueWith(t => LongAdd(t.Result, 3))
                .ContinueWith(t => LongAdd(t.Result, 4))
                .ContinueWith(t => LongAdd(t.Result, 5))
                .ContinueWith(t => Console.WriteLine($"Answer: {t.Result}"));

            var t1 = Task.Run(() => LongAdd(1, 2));
            t1.ContinueWith(t => LongAdd(t.Result, 3));
            t1.ContinueWith(t => LongAdd(t.Result, 4));
            t1.ContinueWith(t => LongAdd(t.Result, 5));
            t1.ContinueWith(t => Console.WriteLine($"Answer: {t.Result}"));
        }
        private static void CancelTask()
        {
            CancellationTokenSource remote = new CancellationTokenSource();
            CancellationToken token = remote.Token;

            Task.Run(() =>
            {
                for (int i = 0; ; i++)
                {
                    if (token.CanBeCanceled && token.IsCancellationRequested)
                    {
                        Console.WriteLine("Bye!");
                        return;
                    }
                    //token.ThrowIfCancellationRequested();
                    Console.WriteLine(i);
                    Task.Delay(1000).Wait();
                }
            });

            remote.CancelAfter(6000);
        }
        private static void ExceptionTask()
        {
            Task.Run(() =>
            {
                Console.WriteLine("Executing...");
                throw new Exception("Ooops");
            }).ContinueWith(t =>
            {
                Console.WriteLine($"State: {t.Status}");
                if (t.Exception != null)
                {
                    Console.WriteLine(t.Exception.InnerException?.Message);
                }
            });
        }
        private static void AwaiterTask()
        {
            var t1 = new Task<int>(() => LongAdd(2, 3));
            t1.Start();

            var waiter = t1.GetAwaiter();

            waiter.OnCompleted(() =>
            {
                int result = waiter.GetResult();
                Console.WriteLine($"Answer: {result}");
            });
        }
        private static async void AwaitTask()
        {
            var t1 = new Task<int>(() => LongAdd(2, 3));
            t1.Start();

            var result = await t1;

            Console.WriteLine($"Answer: {result}");

            result = await Task.Run(() => LongAdd(4, 5));
            Console.WriteLine($"Answer: {result}");

            result = await LongAddAsync(5, 6);
            Console.WriteLine($"Answer: {result}");
        }
        private static async void AwaitExceptionTask()
        {
            try
            {
                await Task.Run(() =>
                {
                    Console.WriteLine("Executing...");
                    throw new Exception("Ooops");
                });
            }
            catch(Exception e)
            { 
                Console.WriteLine(e.Message);
            }
        }
        private static async void LegacyTasks()
        {
            var result = await StartLegacyCode();
            Console.WriteLine(result);


            Task<int> StartLegacyCode()
            {
                var tcs = new TaskCompletionSource<int>();
                ThreadPool.QueueUserWorkItem(x =>
                {
                    Console.WriteLine("Some Legacy Code Started");
                    Thread.Sleep(2000);
                    tcs.TrySetResult(42);
                });
                return tcs.Task;
            }
        }

        private static int LongAdd(int a, int b)
        {
            Task.Delay(5000).Wait();
            return a + b;
        }
        private static Task<int> LongAddAsync(int a, int b)
        {
            return Task.Run(()=>LongAdd(a, b));
        }
    }
}
