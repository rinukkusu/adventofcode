using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2023._09;

public class Day09
{
    [Theory]
    [InlineData(Data.Example01, 114)]
    [InlineData(Data.PuzzleInput, 1921197370)]
    public void Test1(string data, int expectedResult)
    {
        var listOfNumbers = data.Trim().Split(Environment.NewLine)
            .Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries))
            .Select(x => x.Select(int.Parse).ToList())
            .ToList();

        var result = 0;

        foreach (var numbers in listOfNumbers)
        {
            List<List<int>> stepsToZero = new() { numbers };

            while (stepsToZero.Last().Any(x => x != 0))
            {
                var lastEntry = stepsToZero.Last();
                var nextEntry = new List<int>();
                
                for (var i = 0; i < lastEntry.Count - 1; i++)
                {
                    nextEntry.Add(lastEntry[i + 1] - lastEntry[i]);
                }
                
                stepsToZero.Add(nextEntry);
            }

            for (var i = stepsToZero.Count - 1; i > 0; i--)
            {
                stepsToZero[i - 1].Add(stepsToZero[i - 1].Last() + stepsToZero[i].Last());
            }

            result += stepsToZero.First().Last();
        }

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(Data.Example01, 2)]
    [InlineData(Data.PuzzleInput, 1124)]
    public void Test2(string data, int expectedResult)
    {
        var listOfNumbers = data.Trim().Split(Environment.NewLine)
            .Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries))
            .Select(x => x.Select(int.Parse).ToList())
            .ToList();

        var result = 0;

        foreach (var numbers in listOfNumbers)
        {
            List<List<int>> stepsToZero = new() { numbers };

            while (stepsToZero.Last().Any(x => x != 0))
            {
                var lastEntry = stepsToZero.Last();
                var nextEntry = new List<int>();
                
                for (var i = 0; i < lastEntry.Count - 1; i++)
                {
                    nextEntry.Add(lastEntry[i + 1] - lastEntry[i]);
                }
                
                stepsToZero.Add(nextEntry);
            }

            for (var i = stepsToZero.Count - 1; i > 0; i--)
            {
                stepsToZero[i - 1].Insert(0, stepsToZero[i - 1].First() - stepsToZero[i].First());
            }

            result += stepsToZero.First().First();
        }

        Assert.Equal(expectedResult, result);
    }
}