using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2023._04
{
    public class Day04
    {
        [Theory]
        [InlineData(Data.Example01, 13)]
        [InlineData(Data.PuzzleInput, 24160)]
        public void Test1(string data, int expectedResult)
        {
            var result = data.Trim().Split(Environment.NewLine)
                .Select(Card.FromLine)
                .Select(x => x.GetPoints())
                .Sum();

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(Data.Example01, 30)]
        [InlineData(Data.PuzzleInput, 5659035)]
        public void Test2(string data, int expectedResult)
        {
            var cards = data.Trim().Split(Environment.NewLine)
                .Select(Card.FromLine);

            var cardCounts = cards.ToDictionary(x => x.Id, x => 1);

            foreach (var card in cards)
            {
                for (var c = 0; c < cardCounts[card.Id]; c++)
                {
                    var matches = card.GetMatchCount();
                    for (var i = 1; i <= matches; i++)
                    {
                        cardCounts[card.Id + i]++;
                    }
                }
            }

            var result = cardCounts.Values.Sum();

            Assert.Equal(expectedResult, result);
        }
    }
}
