using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using M9_WCF_Client.MathLib;

namespace _9_WCF_Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
           // ConcurrentService();
            AsyncClient();

            Console.ReadLine();
        }

        private static async void AsyncClient()
        {
            MathClient client = new MathClient();
            client.BeginAdd(7, 4, ar => {
                var c2 = ar.AsyncState as MathClient;
                int result = c2.EndAdd(ar);
                Console.WriteLine($"Answer Add: {result}");
            }, client);

            int res = await Task.Factory.FromAsync(client.BeginAdd(17, 3, null, null), client.EndAdd);
            Console.WriteLine($"Answer Add: {res}");

            res = client.Subtract(5, 9);
            Console.WriteLine($"Answer Subtract: {res}");

            client.BeginSubtract(7, 4, ar => {
                var c2 = ar.AsyncState as MathClient;
                int result = c2.EndSubtract(ar);
                Console.WriteLine($"Answer Subtract: {result}");
            }, client);

            res = await Task.Factory.FromAsync(client.BeginSubtract(17, 3, null, null), client.EndSubtract);
            Console.WriteLine($"Answer Subtract: {res}");
        }

        private static void ConcurrentService()
        {
            MathClient client = new MathClient();
            int res = client.Add(4, 5);
            Console.WriteLine(res);
            res = client.Add(14, 15);
            Console.WriteLine(res);
            res = client.Add(24, 25);
            Console.WriteLine(res);
            client.Close();

            client = new MathClient();
            res = client.Add(4, 5);
            Console.WriteLine(res);
            res = client.Add(14, 15);
            Console.WriteLine(res);
            res = client.Add(24, 25);
            Console.WriteLine(res);
            client.Close();
        }
    }
}
