using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2023._07;

public class Day07
{
    [Theory]
    [InlineData(Data.Example, 6440)]
    [InlineData(Data.PuzzleInput, 253603890)]
    public void Test1(string data, int expectedResult)
    {
        var hands = data.Trim().Split(Environment.NewLine)
            .Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries))
            .Select(x => new Hand(x[0], int.Parse(x[1]), withJoker: false))
            .ToList();

        hands.Sort();

        var result = hands.Select((x, idx) => x.Bid * (idx + 1)).Sum();

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(Data.Example, 5905)]
    public void Test2(string data, int expectedResult)
    {
        var hands = data.Trim().Split(Environment.NewLine)
            .Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries))
            .Select(x => new Hand(x[0], int.Parse(x[1]), withJoker: true))
            .ToList();

        hands.Sort();

        var result = hands.Select((x, idx) => x.Bid * (idx + 1)).Sum();

        Assert.Equal(expectedResult, result);
    }
}
