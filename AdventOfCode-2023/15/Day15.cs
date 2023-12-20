using System.Diagnostics;
using Xunit.Abstractions;

namespace AdventOfCode_2023._15;

public class Day15
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Day15(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData(Data.Example01, 1320)]
    [InlineData(Data.PuzzleInput, 511498)]
    public void Test1(string data, int expectedResult)
    {
        var strings = data.Trim().Replace(Environment.NewLine, "")
            .Split(",")
            .ToList();

        var result = strings.Select(HashString).Sum();

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(Data.Example01, 145)]
    [InlineData(Data.PuzzleInput, 284674)]
    public void Test2(string data, int expectedResult)
    {
        var strings = data.Trim().Replace(Environment.NewLine, "")
            .Split(",")
            .ToList();

        var boxes = new List<Operation>[255]
            .Select(x => new List<Operation>())
            .ToList();
        
        foreach (var operation in strings)
        {
            var hash = -1;
            
            if (operation.Contains('='))
            {
                var split = operation.Split("=");
                hash = HashString(split[0]);
                
                var newOp = new Operation(split[0], int.Parse(split[1]));
                var op = boxes[hash].FirstOrDefault(x => x.Label == newOp.Label);
                
                if (op is not null)
                {
                    op.FocalLength = newOp.FocalLength;
                }
                else
                {
                    boxes[hash].Add(newOp);
                }
                continue;
            }

            var label = operation.Replace("-", "");
            hash = HashString(label);
            for (var i = boxes[hash].Count - 1; i >= 0; i--)
            {
                if (boxes[hash][i].Label == label)
                {
                    boxes[hash].RemoveAt(i);
                    break;
                }
            }
        }

        _testOutputHelper.WriteLine(
            string.Join(
                Environment.NewLine, 
                boxes.Select((box, boxIdx) =>
                        $"Box {boxIdx}: {string.Join(" ", box.Select(op => $"[{op.Label} {op.FocalLength}]"))}")
                    .Where(x => x.Contains('['))));

        var result = boxes
            .Select((box, boxIdx) =>
                box.Select((op, opIdx) =>
                {
                    var r = (boxIdx + 1) * (opIdx + 1) * op.FocalLength;
                    return r;
                })).Sum(x => x.Sum());

        Assert.Equal(expectedResult, result);
    }

    [DebuggerDisplay("{Label} {FocalLength}")]
    public class Operation(string label, int focalLength)
    {
        public string Label { get; set; } = label;
        public int FocalLength { get; set; } = focalLength;
        
        public virtual bool Equals(Operation? other)
        {
            if (other is null) return false;

            return Label == other.Label;
        }

        public override int GetHashCode()
        {
            return Label.GetHashCode();
        }
    };

    private static int HashString(string input)
    {
        var hashValue = 0;

        foreach (var c in input)
        {
            hashValue += c;
            hashValue *= 17;
            hashValue %= 256;
        }

        return hashValue;
    }
}