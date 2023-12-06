using Xunit.Abstractions;

namespace AdventOfCode_2023._05;

public class Day05
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Day05(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData(Data.Example01, 35)]
    [InlineData(Data.PuzzleInput, 1181555926)]
    public void Test1(string data, int expectedResult)
    {
        var lines = data.Trim().Split(Environment.NewLine).ToList();

        var seeds = lines[0].Split(":")[1].Trim().Split(" ").Select(long.Parse);

        var mapperPipeline = new MapperPipeline(lines)
            .AddMapper("seed-to-soil")
            .AddMapper("soil-to-fertilizer")
            .AddMapper("fertilizer-to-water")
            .AddMapper("water-to-light")
            .AddMapper("light-to-temperature")
            .AddMapper("temperature-to-humidity")
            .AddMapper("humidity-to-location");

        var result = seeds.Select(x => mapperPipeline.Map(x)).Min();

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(Data.Example01, 46)]
    public void Test2(string data, int expectedResult)
    {
        var lines = data.Trim().Split(Environment.NewLine).ToList();

        var seeds = lines[0].Split(":")[1].Trim().Split(" ").Select(long.Parse).ToList();

        var mapperPipeline = new MapperPipeline(lines)
            .AddMapper("seed-to-soil")
            .AddMapper("soil-to-fertilizer")
            .AddMapper("fertilizer-to-water")
            .AddMapper("water-to-light")
            .AddMapper("light-to-temperature")
            .AddMapper("temperature-to-humidity")
            .AddMapper("humidity-to-location");

        _testOutputHelper.WriteLine(mapperPipeline.ToString());

        var seedPairs = seeds.Where((x, i) => i % 2 == 0) // Select every even indexed element
            .Select((x, i) => (x, seeds[i * 2 + 1])); // Pair it with the next element

        var result = long.MaxValue;

        foreach (var (seed, range) in seedPairs)
        {
            var enumerable = new LongRangeEnumerable(seed, range);

            var lowest = enumerable.AsParallel()
                .Select(testSeed => mapperPipeline.Map(testSeed))
                .Prepend(long.MaxValue).Min();

            _testOutputHelper.WriteLine(lowest.ToString());

            if (lowest < result)
                result = lowest;
        }

        Assert.Equal(expectedResult, result);
    }
}