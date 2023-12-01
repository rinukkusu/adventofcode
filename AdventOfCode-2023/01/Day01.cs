namespace AdventOfCode_2023._01;

public class Day01
{
    [Theory]
    [InlineData(Data.Example01, 142)]
    [InlineData(Data.PuzzleInput, 54390)]
    public void Test1(string data, int expectedResult)
    {
        var result = data.Trim().Split(Environment.NewLine)
            .Select(x => x.Where(c => char.IsDigit(c)).ToArray())
            .Select(x => $"{x.First()}{x.Last()}")
            .Select(int.Parse)
            .Sum();

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(Data.Example02, 281)]
    [InlineData(Data.PuzzleInput, 54277)]
    public void Test2(string data, int expectedResult)
    {
        var result = data.Trim().Split(Environment.NewLine)
            .Select(x => $"{GetFirstNumber(x)}{GetLastNumber(x)}")
            .Select(int.Parse)
            .Sum();

        Assert.Equal(expectedResult, result);
    }

    private int? GetFirstNumber(string text)
    {
        int i = 0;
        int take = 1;

        do
        {
            var sample = text.Substring(i, take);
            var result = FindNumber(sample);

            if (result.found)
            {
                return result.number;
            }

            take++;
        } while ((i + take) <= text.Length);

        return null;
    }

    private int? GetLastNumber(string text)
    {
        int i = text.Length - 1;
        int take = 1;

        do
        {
            var sample = text.Substring(i, take);
            var result = FindNumber(sample);

            if (result.found)
            {
                return result.number;
            }

            i--;
            take++;
        } while ((text.Length - take) >= 0);

        return null;
    }

    private Dictionary<string, int> numberPairs = new Dictionary<string, int>
    {
        { "one", 1 }, { "two", 2 },{ "three", 3 },{ "four", 4 },{ "five", 5 },
        { "six", 6 }, { "seven", 7 },{ "eight", 8 },{ "nine", 9 },
    };

    private (bool found, int? number) FindNumber(string sample)
    {
        if (sample.Any(char.IsDigit))
        {
            return (true, int.Parse(sample.FirstOrDefault(char.IsDigit).ToString()));
        }

        var numberPair = numberPairs.FirstOrDefault(x => sample.Contains(x.Key));
        if (numberPair.Key is null)
            return (false, null);

        return (true, numberPair.Value);
    }
}
