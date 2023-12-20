namespace AdventOfCode_2023._11;

public class Day11
{
    public const char GALAXY = '#';
    public const char EMPTY = '.';
    
    [Theory]
    [InlineData(Data.Example01, 374)]
    [InlineData(Data.PuzzleInput, 10292708)]
    public void Test1(string data, int expectedResult)
    {
        var map = data.Trim().Split(Environment.NewLine)
            .ToList();

        var result = CalculateSumOalaxyDistances(map, 1);

        Assert.Equal(expectedResult, result);
    }
    
    [Theory]
    [InlineData(Data.Example01, 2, 374)]
    [InlineData(Data.Example01, 10, 1030)]
    [InlineData(Data.Example01, 100, 8410)]
    [InlineData(Data.PuzzleInput, 1_000_000, 790194712336)]
    public void Test2(string data, int spaceMultiplicator, int expectedResult)
    {
        var map = data.Trim().Split(Environment.NewLine)
            .ToList();

        var result = CalculateSumOalaxyDistances(map, spaceMultiplicator);

        Assert.Equal(expectedResult, result);
    }

    private long CalculateSumOalaxyDistances(List<string> map, int spaceMultiplicator)
    {
        // scan for empty space in columns
        var emptyColumns = new List<int>();
        for (var i = 0; i < map[0].Length; i++)
        {
            if (map.Any(x => x[i] == GALAXY)) continue;

            emptyColumns.Add(i);
        }
        
        // scan for empty space in rows
        var emptyRows = new List<int>();
        for (var i = 0; i < map.Count; i++)
        {
            if (map[i].Any(x => x == GALAXY)) continue;

            emptyRows.Add(i);
        }
        
        var galaxies = map
            .Select((row, y) => 
                row.Select((ch, x) => 
                    ch == GALAXY
                        ? new Point(x, y) 
                        : new Point(-1, -1)))
            .SelectMany(x => x)
            .Where(coords => coords.X >= 0)
            .Select(p => p with { X = p.X + emptyColumns.Count(c => c < p.X) * (spaceMultiplicator - 1) })
            .Select(p => p with { Y = p.Y + emptyRows.Count(c => c < p.Y) * (spaceMultiplicator - 1) })
            .ToList();

        long result = 0;
        var pairs = new List<(Point, Point)>();
        for (var i = 0; i < galaxies.Count; i++)
        {
            for (var j = 0; j < galaxies.Count; j++)
            {
                if (i == j) continue;
                if (pairs.Any(p => p.Item1 == galaxies[j] && p.Item2 == galaxies[i])) continue;

                pairs.Add((galaxies[i], galaxies[j]));
                result += Math.Abs(galaxies[i].X - galaxies[j].X) + Math.Abs(galaxies[i].Y - galaxies[j].Y);
            }
        }

        return result;
    }

    public record Point(long X, long Y);
}