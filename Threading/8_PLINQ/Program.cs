using Bogus;
using System.Diagnostics;
using System.Net.Http.Json;

namespace M8_PLINQ
{
    internal class Program
    {
        static readonly Random rnd = new Random();
        static readonly List<Person> people;

        static void Main(string[] args)
        {
            //CheckCCSync();
            CheckCCParallel();
            //CheckCCCancel();
            //TestMerge();
            Console.ReadLine();
        }


        private static void CheckCCSync()
        {
            var q = people
                .Where(p => !IsValidCreditCard(p.CreditCardNumber));

            var sw = new Stopwatch();
            sw.Start();
            foreach (var p in q)
            {
                Console.WriteLine($"{p.FirstName} {p.LastName} has an invalid CreditCard");
            }
            sw.Stop();
            Console.WriteLine($"Elapsed Synchronous: {sw.Elapsed}");
            Console.WriteLine($"Nr of threads used {ThreadPool.ThreadCount}");
        }
        private static async void CheckCCParallel()
        {
            var query = people
                .AsParallel()
               //.WithDegreeOfParallelism(2)
               //.AsOrdered()
               //.AsUnordered()
               //.WithExecutionMode(ParallelExecutionMode.ForceParallelism) // Forces parallelism. Otherwise a guess is made
               .Where(p => !IsValidCreditCard(p.CreditCardNumber));
            // From package System.Linq.Async
            // .ToAsyncEnumerable()
            //.WhereAwait(async p => !(await IsValidCreditCardRemote(p.CreditCardNumber)));
            //.AsSequential()
            //.Where(p => p.Age > 50);

            var sw = new Stopwatch();
            sw.Start();
            //await foreach (var p in query)
            foreach (var p in query)
            {
                Console.WriteLine($"[{p.Id}] {p.FirstName} {p.LastName} has an invalid CreditCard");
            }

            sw.Stop();
            Console.WriteLine($"Elapsed Synchronous: {sw.Elapsed}");
            Console.WriteLine($"Nr of threads used {ThreadPool.ThreadCount}");
        }
        private static void CheckCCCancel()
        {
            CancellationTokenSource nikko = new CancellationTokenSource();
            var q = people
                .AsParallel()
                .WithCancellation(nikko.Token)
                .Where(p => !IsValidCreditCard(p.CreditCardNumber));

            var sw = new Stopwatch();
            nikko.CancelAfter(5000);
            sw.Start();
            foreach (var p in q)
            {
                Console.WriteLine($"[{p.Id}] {p.FirstName} {p.LastName} has an invalid CreditCard");
            }
            sw.Stop();
            Console.WriteLine($"Elapsed Synchronous: {sw.Elapsed}");
            Console.WriteLine($"Nr of threads used {ThreadPool.ThreadCount}");
        }
        private static void TestMerge()
        {
            var q = Numbers()
                .AsParallel()
                .WithMergeOptions(ParallelMergeOptions.NotBuffered);

            foreach (var nr in q)
            {
                Console.WriteLine($"Consuming {nr}");
            }

            IEnumerable<int> Numbers()
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine($"Producing {i}");
                    yield return i;
                    Task.Delay(10).Wait();
                }
            }
        }

        static HttpClient client = new HttpClient { BaseAddress = new Uri("https://localhost:8001") };
        static async Task<bool> IsValidCreditCardRemote(string? cc)
        {
            //var sockopts = new SocketsHttpHandler
            //{
            //    ConnectTimeout = TimeSpan.FromMinutes(4),
            //    MaxConnectionsPerServer = 25 // Number of ports opened by HttpClient
            //};
            //client = new HttpClient(sockopts) {BaseAddress = new Uri("https://localhost:8001") };

            // netstat -a
            //var response = client.GetAsync($"check/{cc}").Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var result = response.Content.ReadAsStringAsync().Result;
            //    bool.TryParse(result, out bool ok);
            //    return ok;
            //}
            //return false;

            var response = await client.GetAsync($"check/{cc}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                bool.TryParse(result, out bool ok);
                return ok;
            }
            return false;
        }
        static bool IsValidCreditCard(string? cc)
        {
            //client.GetStringAsync("checker").ContinueWith();
            // Different providers...
            for (int i = 0; i < 10; i++)
            {
                Task.Delay(100).Wait();
                if (rnd.Next(0, 20) > 18)
                {
                    return false;
                }
            }
            return true;
        }

        static Program()
        {
            people = new Faker<Person>()
                .RuleFor(p => p.Id, f => f.UniqueIndex)
                .RuleFor(p => p.FirstName, f => f.Person.FirstName)
                .RuleFor(p => p.LastName, f => f.Person.LastName)
                .RuleFor(p => p.Age, f => f.Random.Int(0, 123))
                .RuleFor(p => p.CreditCardNumber, f => f.Finance.CreditCardNumber())
                .Generate(100)
                .ToList();

        }
    }
}
