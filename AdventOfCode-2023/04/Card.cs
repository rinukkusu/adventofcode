using System.Text.RegularExpressions;

namespace AdventOfCode_2023._04
{
    public partial class Card
    {
        [GeneratedRegex(@"^Card\s+?(?<card>\d+)\:\s(?<winning>[^|]+)\|(?<actual>.+)$")]
        private static partial Regex CardRegex();

        public int Id { get; set; }
        public int[] WinningNumbers { get; set; } = [];
        public int[] ActualNumbers { get; set; } = [];

        public static Card FromLine(string line)
        {
            var match = CardRegex().Match(line);
            if (!match.Success)
                throw new Exception($"Line didn't match Regex: {line}");

            return new Card
            {
                Id = int.Parse(match.Groups["card"].Value),
                WinningNumbers = match.Groups["winning"].Value.Trim()
                    .Split(' ')
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(int.Parse)
                    .ToArray(),
                ActualNumbers = match.Groups["actual"].Value.Trim()
                    .Split(' ')
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(int.Parse)
                    .ToArray(),
            };
        }

        public int GetPoints()
        {
            var points = 0;

            foreach (var number in ActualNumbers)
            {
                if (WinningNumbers.Contains(number))
                {
                    if (points == 0)
                        points = 1;
                    else
                        points *= 2;
                }
            }

            return points;
        }

        public int GetMatchCount()
        {
            return ActualNumbers.Where(x => WinningNumbers.Contains(x)).Count();
        }
    }
}
