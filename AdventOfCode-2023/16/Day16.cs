using System.Drawing;

namespace AdventOfCode_2023._16;

public class Day16
{
    [Theory]
    [InlineData(Data.Example01, 46)]
    [InlineData(Data.PuzzleInput, 7608)]
    public void Test1(string data, int expectedResult)
    {
        var input = data.Trim().Split(Environment.NewLine).ToArray();

        var result = GetEnergizedTiles(input, new Point(0, 0), Direction.Right);

        Assert.Equal(expectedResult, result);
    }
    
    [Theory]
    [InlineData(Data.Example01, 51)]
    [InlineData(Data.PuzzleInput, 8221)]
    public void Test2(string data, int expectedResult)
    {
        var input = data.Trim().Split(Environment.NewLine).ToArray();

        var result = 0;

        for (var i = 0; i < input.Length; i++)
        {
            // from left
            var energizedTiles = GetEnergizedTiles(input, new Point(0, i), Direction.Right);
            if (energizedTiles > result)
                result = energizedTiles;
            
            // from right
            energizedTiles = GetEnergizedTiles(input, new Point(input[0].Length - 1, i), Direction.Left);
            if (energizedTiles > result)
                result = energizedTiles;
        }
        
        for (var i = 0; i < input[0].Length; i++)
        {
            // from top
            var energizedTiles = GetEnergizedTiles(input, new Point(i, 0), Direction.Down);
            if (energizedTiles > result)
                result = energizedTiles;
            
            // from bottom
            energizedTiles = GetEnergizedTiles(input, new Point(i, input.Length), Direction.Left);
            if (energizedTiles > result)
                result = energizedTiles;
        }

        Assert.Equal(expectedResult, result);
    }

    private static int GetEnergizedTiles(string[] input, Point startingPoint, Direction startingDirection)
    {
        var matrix = input.Select(line => line.Select(t => new Tile(t, 0)).ToArray())
            .ToArray();

        var operations = new Queue<(Point point, Direction direction)>();
        operations.Enqueue((startingPoint, startingDirection));

        while (operations.Count > 0)
        {
            var op = operations.Dequeue();
            var nextOperations = GetNextDirections(matrix, op.point, op.direction);
            
            foreach (var nextOp in nextOperations)
            {
                operations.Enqueue(nextOp);
            }
        }

        return matrix.Sum(x => x.Sum(t => t.Level > 0 ? 1 : 0));
    }
    
    private static IEnumerable<(Point point, Direction direction)> GetNextDirections(Tile[][] matrix, Point p, Direction d)
    {
        if (IsOutsideBounds(matrix, p))
            return Array.Empty<(Point point, Direction direction)>();
        
        var tile = matrix[p.Y][p.X];
        
        if (!tile.IncreaseLevel(d))
            return Array.Empty<(Point point, Direction direction)>();

        var nextDirections = tile.GetDirections(d);

        return nextDirections
            .Select(x => (GetPointFromDirection(p, x), x))
            .Where(x => !IsOutsideBounds(matrix, x.Item1))
            .ToList();
    }

    private static bool IsOutsideBounds(Tile[][] matrix, Point p)
    {
        return p.Y < 0 || p.Y >= matrix.Length || p.X < 0 || p.X >= matrix[0].Length;
    }

    private static Point GetPointFromDirection(Point p, Direction d)
    {
        return d switch
        {
            Direction.Right => p with { X = p.X + 1 },
            Direction.Down => p with { Y = p.Y + 1 },
            Direction.Left => p with { X = p.X - 1 },
            Direction.Up => p with { Y = p.Y - 1 },
            _ => throw new ArgumentOutOfRangeException(nameof(d))
        };
    }

    public enum Direction
    {
        Right, Down, Left, Up
    }

    public class Tile(char operation, int level)
    {
        public char Operation { get; } = operation;

        public int Level { get; private set; } = level;

        private readonly HashSet<Direction> _directionHistory = [];

        public bool IncreaseLevel(Direction comingFrom)
        {
            if (!_directionHistory.Add(comingFrom))
                return false;

            Level++;
            return true;
        }

        public IEnumerable<Direction> GetDirections(Direction current)
        {
            return Operation switch
            {
                '/' => [current switch
                {
                    Direction.Down => Direction.Left,
                    Direction.Left => Direction.Down,
                    Direction.Up => Direction.Right,
                    _ => Direction.Up
                }],
                '\\' => [current switch
                {
                    Direction.Down => Direction.Right,
                    Direction.Right => Direction.Down,
                    Direction.Up => Direction.Left,
                    _ => Direction.Up
                }],
                '-' => current switch
                {
                    Direction.Down or Direction.Up => [Direction.Left, Direction.Right],
                    _ => [current]
                },
                '|' => current switch
                {
                    Direction.Left or Direction.Right => [Direction.Up, Direction.Down],
                    _ => [current]
                },
                
                _ => [current]
            };
        }
    }
}