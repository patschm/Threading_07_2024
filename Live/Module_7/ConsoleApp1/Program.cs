using System.Collections.Concurrent;

namespace ConsoleApp1;

internal class Program
{
    static void Main(string[] args)
    {
        DoeIets();
        Console.WriteLine("Hello, World!");
    }

    static async void DoeIets()
    {
        Console.WriteLine("Doe iets");
    }
}
