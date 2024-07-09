namespace M10_Testing;

public class Calculator
{
    public async Task<int> AddAsync(int a, int b)
    {
        return await Task.Run(async () => {
            await Task.Delay(5000);
            return a + b;
        });
    }
}
