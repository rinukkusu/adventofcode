namespace AdventOfCode_2023._02;

public class Game(int id, int red, int green, int blue)
{
    public int Id { get; } = id;
    public int Red { get; } = red;
    public int Green { get; } = green;
    public int Blue { get; } = blue;

    public static Game FromString(string line)
    {
        var gameSplit = line.Split(":");

        var id = int.Parse(gameSplit[0].Trim().Split(" ")[1]);
        var red = 0;
        var green = 0;
        var blue = 0;

        var rounds = gameSplit[1].Split(";");
        foreach (var round in rounds)
        {
            var draws = round.Split(",");
            foreach (var draw in draws)
            {
                var drawSplit = draw.Trim().Split(" ");
                var cubeCount = int.Parse(drawSplit[0]);
                var cubeColor = drawSplit[1];

                if (cubeColor == "red")
                    red = cubeCount > red ? cubeCount : red;
                else if (cubeColor == "green")
                    green = cubeCount > green ? cubeCount : green;
                else if (cubeColor == "blue")
                    blue = cubeCount > blue ? cubeCount : blue;
                else
                    throw new Exception(@$"What is this color '{cubeColor}'?
{line}");
            }
        }

        return new Game(id, red, green, blue);
    }
}