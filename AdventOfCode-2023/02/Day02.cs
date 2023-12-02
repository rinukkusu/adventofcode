namespace AdventOfCode_2023._02;

public class Day02
{
    [Theory]
    [InlineData(Data.Example01, 12, 13, 14, 8)]
    [InlineData(Data.PuzzleInput, 12, 13, 14, 2593)]
    public void Test1(string data, int maxRed, int maxGreen, int maxBlue, int expectedResult)
    {
        var games = data.Trim()
            .Split(Environment.NewLine)
            .Select(Game.FromString);
        
        var result = games.Where(x => x.Red <= maxRed && x.Green <= maxGreen && x.Blue <= maxBlue)
            .Sum(x => x.Id);
        
        Assert.Equal(expectedResult, result);
    }
    
    [Theory]
    [InlineData(Data.Example01, 2286)]
    [InlineData(Data.PuzzleInput, 54699)]
    public void Test2(string data, int expectedResult)
    {
        var games = data.Trim()
            .Split(Environment.NewLine)
            .Select(Game.FromString);
        
        var result = games
            .Sum(x => x.Red * x.Green * x.Blue);
        
        Assert.Equal(expectedResult, result);
    }
}