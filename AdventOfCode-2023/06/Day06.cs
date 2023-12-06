using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2023._06;

public class Day06
{
    [Theory]
    [InlineData(Data.Example, 288)]
    [InlineData(Data.PuzzleInput, 160816)]
    public void Test1(string data, int expectedResult)
    {
        var rawRaces = data.Trim().Split(Environment.NewLine)
            .Select(x => x.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            .Select(x => x.Skip(1).Select(int.Parse).ToList())
            .ToList();

        // pivot list
        var races = rawRaces[0].Select((x, idx) => (time: x, distance: rawRaces[1][idx]));

        var result = races
            .Select(x => CalculateWinners(x.time, x.distance))
            .Aggregate((a, b) => a * b);

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(Data.Example, 71503)]
    [InlineData(Data.PuzzleInput, 46561107)]
    public void Test2(string data, int expectedResult)
    {
        var rawRaces = data.Trim().Split(Environment.NewLine)
            .Select(x => x.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            .Select(x => x.Skip(1))
            .Select(x => string.Join("", x))
            .Select(long.Parse)
            .ToList();

        // pivot list
        var (time, distance) = (rawRaces[0], rawRaces[1]);

        var result = CalculateWinners(time, distance);

        Assert.Equal(expectedResult, result);
    }

    private long CalculateWinners(long time, long distance)
    {
        long winnersFound = 0;

        for (long buttonHeldTime = 0; buttonHeldTime <= time; buttonHeldTime++)
        {
            var currentSpeed = buttonHeldTime;
            var currentDistance = (time - buttonHeldTime) * currentSpeed;

            if (currentDistance > distance)
            {
                winnersFound++;
            }
        }

        return winnersFound;
    }
}