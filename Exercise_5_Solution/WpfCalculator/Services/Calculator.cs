namespace WpfCalculator.Services;

internal class Calculator
{
    public int LongAdd(int a, int b) 
    {
        Task.Delay(10000).Wait();
        return a + b;   
    }
}
