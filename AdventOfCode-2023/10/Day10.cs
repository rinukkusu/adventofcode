namespace AdventOfCode_2023._10;

public class Day10
{
    [Theory]
    [InlineData(Data.Example01, 4)]
    [InlineData(Data.Example02, 8)]
    [InlineData(Data.PuzzleInput, 7107)]
    public void Test1(string data, int expectedResult)
    {
        var mazeInput = data.Trim().Split(Environment.NewLine)
            .ToList();

        var maze = new Maze(mazeInput);

        var result = maze.Solve();

        Assert.Equal(expectedResult, result);
    }
}