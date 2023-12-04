using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2023._03
{
    public partial class Day03
    {
        [Theory]
        [InlineData(Data.Example01, 4361)]
        [InlineData(Data.PuzzleInput, 540212)]
        public void Test1(string data, int expectedResult)
        {
            var matrix = data.Trim().Split(Environment.NewLine)
                .Select(x => x.Select(c => c).ToArray())
                .ToArray();

            var partNumbers = FindPartNumbers(matrix);

            var result = partNumbers.Sum(x => x.PartNumbers.Sum());

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(Data.Example01, 467835)]
        [InlineData(Data.PuzzleInput, 87605697)]
        public void Test2(string data, int expectedResult)
        {
            var matrix = data.Trim().Split(Environment.NewLine)
                .Select(x => x.Select(c => c).ToArray())
                .ToArray();

            var gears = FindPartNumbers(matrix);

            var result = gears
                .Where(x => x.Symbol == '*' && x.PartNumbers.Count == 2)
                .Sum(x => x.PartNumbers.Aggregate((a, b) => a * b));

            Assert.Equal(expectedResult, result);
        }

        public List<Gear> FindPartNumbers(char[][] matrix)
        {
            var result = new List<Gear>();
            void tryAddGear(List<(int X, int Y, char Symbol)> symbols, int partNumber)
            {
                var distinctSymbols = symbols
                    .GroupBy(s => (s.X, s.Y))
                    .Select(g => g.First())
                    .ToList();

                foreach (var symbol in distinctSymbols)
                {
                    var gear = result.FirstOrDefault(g => g.X == symbol.X && g.Y == symbol.Y);
                    if (gear == null)
                    {
                        gear = new Gear(symbol.X, symbol.Y, symbol.Symbol);
                        result.Add(gear);
                    }

                    gear.PartNumbers.Add(partNumber);
                }
            }

            for (int y = 0; y < matrix.Length; y++)
            {
                var foundSymbols = new List<(int X, int Y, char Symbol)>();
                var foundPartNumber = "0";

                for (int x = 0; x < matrix[0].Length; x++)
                {
                    var value = matrix[y][x];

                    if (char.IsDigit(value))
                    {
                        var foundSymbolAt = FindSymbolAndPosition(matrix, x, y);
                        if (foundSymbolAt.X >= 0)
                        {
                            foundSymbols.Add(foundSymbolAt);
                        }
                        foundPartNumber += value;
                    }
                    else
                    {
                        if (foundSymbols.Any())
                        {
                            tryAddGear(foundSymbols, int.Parse(foundPartNumber));
                        }

                        foundSymbols.Clear();
                        foundPartNumber = "0";
                    }

                    // add potential part numbers, when we're at the last column
                    if (foundSymbols.Any() && x == matrix[0].Length - 1)
                    {
                        tryAddGear(foundSymbols, int.Parse(foundPartNumber));
                    }
                }
                Console.WriteLine();
            }

            return result;
        }

        public (int X, int Y, char Symbol) FindSymbolAndPosition(char[][] matrix, int currentX, int currentY)
        {
            (bool Found, char Symbol) IsSymbol(int x, int y)
            {
                if (x < 0 || y < 0 || x >= matrix[0].Length || y >= matrix.Length)
                    return (false, '\0');

                var symbol = matrix[y][x];
                return (symbol != '.' && !char.IsDigit(symbol), symbol);
            }

            var result = IsSymbol(currentX - 1, currentY);
            if (result.Found)
                return (currentX - 1, currentY, result.Symbol);
            result = IsSymbol(currentX - 1, currentY - 1);
            if (result.Found)
                return (currentX - 1, currentY - 1, result.Symbol);
            result = IsSymbol(currentX, currentY - 1);
            if (result.Found)
                return (currentX, currentY - 1, result.Symbol);
            result = IsSymbol(currentX + 1, currentY - 1);
            if (result.Found)
                return (currentX + 1, currentY - 1, result.Symbol);
            result = IsSymbol(currentX + 1, currentY);
            if (result.Found)
                return (currentX + 1, currentY, result.Symbol);
            result = IsSymbol(currentX + 1, currentY + 1);
            if (result.Found)
                return (currentX + 1, currentY + 1, result.Symbol);
            result = IsSymbol(currentX, currentY + 1);
            if (result.Found)
                return (currentX, currentY + 1, result.Symbol);
            result = IsSymbol(currentX - 1, currentY + 1);
            if (result.Found)
                return (currentX - 1, currentY + 1, result.Symbol);

            return (-1, -1, '\0'); // Indicating no symbol found
        }
    }
}
