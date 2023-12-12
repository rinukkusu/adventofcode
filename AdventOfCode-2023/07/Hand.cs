namespace AdventOfCode_2023._07;

public class Hand(string cards, int bid, bool withJoker) : IComparable<Hand>
{
    public string Cards { get; } = cards;

    public HandType Type { get; } = withJoker ? GetHandTypeWithJokers(cards) : GetHandType(cards);

    public int Bid { get; } = bid;

    public bool WithJoker { get; } = withJoker;

    private static HandType GetHandType(string hand)
    {
        var frequency = new Dictionary<char, int>();

        foreach (char card in hand)
        {
            if (frequency.ContainsKey(card))
                frequency[card]++;
            else
                frequency[card] = 1;
        }

        if (frequency.ContainsValue(5))
            return HandType.FiveOfAKind;
        if (frequency.ContainsValue(4))
            return HandType.FourOfAKind;
        if (frequency.ContainsValue(3) && frequency.ContainsValue(2))
            return HandType.FullHouse;
        if (frequency.ContainsValue(3))
            return HandType.ThreeOfAKind;
        if (frequency.Values.Count(v => v == 2) == 2)
            return HandType.TwoPair;
        if (frequency.ContainsValue(2))
            return HandType.OnePair;

        return HandType.HighCard;
    }

    private static HandType GetHandTypeWithJokers(string hand)
    {
        var frequency = new Dictionary<char, int>();
        int jokerCount = hand.Count(c => c == 'J');

        foreach (char card in hand)
        {
            if (card != 'J')
            {
                if (frequency.ContainsKey(card))
                    frequency[card]++;
                else
                    frequency[card] = 1;
            }
        }

        // Check for Five of a Kind
        if (frequency.Any(kv => kv.Value + jokerCount >= 5))
            return HandType.FiveOfAKind;

        // Check for Four of a Kind or Full House
        if (jokerCount > 0)
        {
            if (frequency.Any(kv => kv.Value + jokerCount == 4) ||
                (frequency.Count(kv => kv.Value == 3) == 1 && jokerCount >= 2) ||
                (frequency.Count(kv => kv.Value == 2) >= 1 && jokerCount >= 1))
                return HandType.FourOfAKind;

            if (frequency.Any(kv => kv.Value == 3) ||
                (frequency.Count(kv => kv.Value == 2) >= 2 && jokerCount >= 1) ||
                (frequency.Count(kv => kv.Value == 2) == 1 && jokerCount >= 3))
                return HandType.FullHouse;
        }
        else
        {
            if (frequency.Any(kv => kv.Value == 4))
                return HandType.FourOfAKind;
            if (frequency.Count(kv => kv.Value == 3) == 1 &&
                frequency.Any(kv => kv.Value == 2))
                return HandType.FullHouse;
        }

        // Check for Three of a Kind or Two Pair
        if (frequency.Any(kv => kv.Value + jokerCount == 3))
            return HandType.ThreeOfAKind;
        if (frequency.Count(kv => kv.Value + (kv.Value == 2 ? jokerCount : 0) == 2) >= 2)
            return HandType.TwoPair;

        // Check for One Pair
        if (frequency.Any(kv => kv.Value + jokerCount == 2))
            return HandType.OnePair;

        return HandType.HighCard;
    }

    public int CompareTo(Hand? other)
    {
        if (other == null) return 1;

        // Compare types
        int result = Type.CompareTo(other.Type);
        if (result != 0) return result;

        // Compare single cards from first to last
        for (int i = 0; i < Cards.Length; i++)
        {
            result = GetCardValue(Cards[i]).CompareTo(GetCardValue(other.Cards[i]));
            if (result != 0) return result;
        }

        return 0;
    }

    private int GetCardValue(char card)
    {
        if (char.IsDigit(card))
            return int.Parse(card.ToString());

        return card switch
        {
            'T' => 10,
            'J' => WithJoker ? 1 : 11,
            'Q' => 12,
            'K' => 13,
            'A' => 14,
            _ => throw new NotImplementedException()
        };
    }

    public enum HandType
    {
        HighCard = 0,
        OnePair = 10,
        TwoPair = 20,
        ThreeOfAKind = 30,
        FullHouse = 40,
        FourOfAKind = 50,
        FiveOfAKind = 60
    }
}
