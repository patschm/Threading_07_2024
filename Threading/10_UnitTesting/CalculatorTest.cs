using M10_Testing;

namespace M10_UnitTesting;

public class CalculatorTest
{
    [Theory]
    [InlineData(1,2, 3)]
    [InlineData(8, 6, 14)]
    [InlineData(4, 7, 11)]
    public async Task Test_AddAsync(int a, int b, int expected)
    {
        var client = new Calculator();
        
        int result = await client.AddAsync(a, b);
        Assert.Equal(expected, result);

    }
}