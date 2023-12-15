using System.Diagnostics;
using System.Drawing;

namespace AdventOfCode_2023._10;

[Flags]
public enum MazeDirection
{
    None = 0,
    Top = 1,
    Right = 2,
    Bottom = 4,
    Left = 8
}

public class Maze(List<string> maze)
{
    public int Solve()
    {
        var start = GetStartingPoint();
        var pointPaths = GetNextPoints(start)
            .Select(p => new List<Point> { start, p })
            .ToList();

        var endFound = false;

        while (!endFound)
        {
            foreach (var pointHistory in pointPaths)
            {
                var currentPoint = pointHistory.Last();
                var nextPoints = GetNextPoints(currentPoint);
                var nextPoint = nextPoints.FirstOrDefault(np => !pointHistory.Contains(np));
                if (nextPoint == default)
                {
                    endFound = true;
                    break;
                };
                
                pointHistory.Add(nextPoint);
            }
        }

        return pointPaths[0].Count / 2;
    }

    private IEnumerable<Point> GetNextPoints(Point current)
    {
        var currentSymbol = Get(current);
        var possibleDirections = GetPossibleDirections(currentSymbol);
        
        // check top
        var top = current with { Y = current.Y - 1 };
        if (IsInBounds(top) && possibleDirections.Contains(MazeDirection.Top))
        {
            var symbol = Get(top);
            if (GetPossibleDirections(symbol).Contains(MazeDirection.Bottom))
                yield return top;
        }

        // check right
        var right = current with { X = current.X + 1 };
        if (IsInBounds(right) && possibleDirections.Contains(MazeDirection.Right))
        {
            var symbol = Get(right);
            if (GetPossibleDirections(symbol).Contains(MazeDirection.Left))
                yield return right;
        }
        
        // check bottom
        var bottom = current with { Y = current.Y + 1 };
        if (IsInBounds(bottom) && possibleDirections.Contains(MazeDirection.Bottom))
        {
            var symbol = Get(bottom);
            if (GetPossibleDirections(symbol).Contains(MazeDirection.Top))
                yield return bottom;
        }

        // check left
        var left = current with { X = current.X - 1 };
        if (IsInBounds(left) && possibleDirections.Contains(MazeDirection.Left))
        {
            var symbol = Get(left);
            if (GetPossibleDirections(symbol).Contains(MazeDirection.Right))
                yield return left;
        }
    }

    private MazeDirection[] GetPossibleDirections(char c)
    {
        return c switch
        {
            'S' => [MazeDirection.Bottom, MazeDirection.Left, MazeDirection.Right, MazeDirection.Top],
            'F' => [MazeDirection.Right, MazeDirection.Bottom],
            'J' => [MazeDirection.Left, MazeDirection.Top],
            'L' => [MazeDirection.Right, MazeDirection.Top],
            '7' => [MazeDirection.Left, MazeDirection.Bottom],
            '|' => [MazeDirection.Top, MazeDirection.Bottom],
            '-' => [MazeDirection.Right, MazeDirection.Left],
            _ => []
        };
    }

    private char Get(Point p)
    {
        return maze[p.Y][p.X];
    }

    private bool IsInBounds(Point p)
    {
        return p.Y >= 0 && p.Y < maze.Count &&
               p.X >= 0 && p.X < maze[0].Length;
    }

    private Point GetStartingPoint()
    {
        return maze.Select((row, y) =>
        {
            var x = row.IndexOf('S');
            return x < 0 
                ? new Point(-1, -1) 
                : new Point(x, y);
        }).First(p => p.X >= 0);
    }
}