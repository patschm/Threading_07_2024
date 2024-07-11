﻿using System.IO.Pipes;
using System;
using System.Threading;

namespace SumUp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int a = 0;
            int b = 0;
            
            var sigA = new AutoResetEvent(false);
            var sigB = new AutoResetEvent(false);
            RunService("service1", x =>
            {
                a = x;
                sigA.Set();
            });
            RunService("service2", x => {
                b = x;
                sigB.Set(); 
            });

            var cde = new CountdownEvent(2);
            int sumA = 0;
            new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    sigA.WaitOne();
                    sumA += a;
                    Console.WriteLine(sumA);
                }
                cde.Signal();
            }).Start();

            int sumB = 0;
            new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    sigB.WaitOne();
                    sumB += b;
                    Console.WriteLine(sumB);
                }
                cde.Signal();
            }).Start();

            cde.Wait();
            Console.WriteLine($"Total: {sumA + sumB}");
            Console.ReadLine();            
        }

        static void RunService(string serviceName, Action<int> fn)
        {
            Thread.Sleep(1000);
            new Thread(() =>
            {
                var pipe = new NamedPipeClientStream(".", serviceName);
                pipe.Connect();
                byte[] buffer = new byte[4];
                while (pipe.Read(buffer, 0, 4) > 0)
                {
                    if (!pipe.IsConnected) return;
                    int nr = BitConverter.ToInt32(buffer, 0);
                    Console.WriteLine($"Received from {serviceName}: {nr}");
                    fn(nr);
                }
            }).Start();
        }
    }
}