
using System;
using System.ServiceModel;

namespace M9_WCF_Service
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(MathImp)))
            {
                host.Open();
                Console.WriteLine("Service is running...");
                Console.ReadLine();
                host.Close();
            }
        }
    }
}
