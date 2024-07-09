
namespace M10_Testing;

internal class Program
{
    static async Task Main(string[] args)
    {
        var calc = new Calculator();
        int res = await calc.AddAsync(5, 6);
        Console.WriteLine(res);
        Console.ReadLine(); 
    }
}
