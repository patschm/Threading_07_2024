using System;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace Service_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var rnd = new Random();
            var pipe = new NamedPipeServerStream("service1");

            Console.WriteLine("Waiting for incoming connections");
            pipe.WaitForConnection();
            Console.WriteLine("Connected!");
            for (int i = 1; i <= 10; i++)
            {
                Thread.Sleep(3000 + rnd.Next(500, 2500));
                Console.WriteLine($"Sending {i}");
                pipe.Write(BitConverter.GetBytes(i), 0, 4);
            }
            pipe.Disconnect();
            pipe.Close();
            Console.ReadLine();
        }
    }
}
