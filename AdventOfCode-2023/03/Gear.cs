namespace AdventOfCode_2023._03
{
    public partial class Day03
    {
        public class Gear
        {
            public int X { get; set; }
            public int Y { get; set; }

            public char Symbol { get; set; }

            public List<int> PartNumbers { get; set; } = new List<int>();

            public Gear(int x, int y, char symbol)
            {
                X = x;
                Y = y;
                Symbol = symbol;
            }
        }
    }
}
