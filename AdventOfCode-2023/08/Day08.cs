using AdventOfCode_2023._07;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit.Abstractions;

namespace AdventOfCode_2023._08
{
    public class Day08
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Day08(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(Data.Example01, 2)]
        [InlineData(Data.Example02, 6)]
        [InlineData(Data.PuzzleInput, 16343)]
        public void Test1(string data, int expectedResult)
        {
            var input = data.Trim().Split(Environment.NewLine).ToList();
            var instructions = input[0].Select(x => x == 'L' ? 0 : 1).ToArray();
            var nodeLines = input.Skip(2)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            var nodes = CreateNodes(nodeLines);
            var node = nodes.Values.Single(x => x.IsStart);
            var steps = 0;
            var instructionIndex = 0;

            while (!node.IsEnd)
            {
                var instruction = instructions[instructionIndex];
                var nextNodeIndex = node.QuickOptions[instruction];
                node = nodes[nextNodeIndex];
                steps++;

                instructionIndex++;
                if (instructionIndex > instructions.Length - 1)
                    instructionIndex = 0;
            }

            Assert.Equal(expectedResult, steps);
        }

        [Theory]
        [InlineData(Data.Example03, 6)]
        [InlineData(Data.PuzzleInput, 6)]
        public void Test2(string data, ulong expectedResult)
        {
            var input = data.Trim().Split(Environment.NewLine).ToList();
            var instructions = input[0].Select(x => x == 'L' ? 0 : 1).ToArray();
            var nodeLines = input.Skip(2)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            var nodes = CreateNodes(nodeLines);
            var allStartNodes = nodes.Values.Where(x => x.IsStart).ToList();
            ulong steps = 0;
            var instructionIndex = 0;
            var endsFound = 0;

            while (!allStartNodes.All(x => x.IsEnd))
            {
                var instruction = instructions[instructionIndex];

                for (int i = 0; i < allStartNodes.Count; i++)
                {
                    var nextNodeIndex = allStartNodes[i].QuickOptions[instruction];
                    allStartNodes[i] = nodes[nextNodeIndex];
                }

                steps++;

                instructionIndex++;
                if (instructionIndex > instructions.Length - 1)
                    instructionIndex = 0;

                //if (allStartNodes.Count(x => x.IsEnd) > endsFound)
                //{
                //    endsFound = allStartNodes.Count(x => x.IsEnd);
                //    _testOutputHelper.WriteLine($"Done: {endsFound}/{allStartNodes.Count} at step {steps}");
                //}

                //if (steps % 10000 == 0)
                //{
                //    _testOutputHelper.WriteLine($"Steps: {steps}");
                //}
            }

            Assert.Equal(expectedResult, steps);
        }

        private Dictionary<int, Node> CreateNodes(List<string> lines)
        {
            var regex = new Regex(@"(?<name>^\w+)\s=\s\((?<left>\w+),\s(?<right>\w+)\)");
            var nodeDict = new Dictionary<string, Node>();

            var nodes = lines.Select((x, idx) =>
            {
                var match = regex.Match(x);
                var node = new Node(
                    idx,
                    match.Groups["name"].Value,
                    [
                        match.Groups["left"].Value,
                        match.Groups["right"].Value
                    ]);
                nodeDict.Add(node.Name, node);
                return node;
            }).ToArray();

            foreach (var node in nodes)
            {
                node.UpdateOptions(nodeDict);
            }

            return nodes.ToDictionary(x => x.Index, x => x);
        }
    }

    public class Node(int index, string name, string[] options)
    {
        public int Index { get; init; } = index;
        public string Name { get; init; } = name;
        public string[] Options { get; init; } = options;

        public bool IsStart = name.EndsWith('A');
        public bool IsEnd = name.EndsWith('Z');

        public int[] QuickOptions { get; private set; } = [];

        public void UpdateOptions(Dictionary<string, Node> lookup)
        {
            QuickOptions =
            [
                lookup[Options[0]].Index,
                lookup[Options[1]].Index
            ];
        }
    }
}
