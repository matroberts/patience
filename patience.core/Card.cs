using System;
using System.Text.RegularExpressions;

namespace patience.core
{
    public enum Suit
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }

    public enum Color
    {
        Red,
        Black
    }

    public struct Card
    {
        public Card(Suit suit, int rank)
        {
            if(rank < 1 || rank > 13)
                throw new ArgumentException($"Rank 'rank' is outside the allowed range which is 1-13");
            Suit = suit;
            Rank = rank;
        }
        public Suit Suit;
        public int Rank;

        public Color Color =>
            Suit switch
            {
                Suit.Clubs => Color.Black,
                Suit.Diamonds => Color.Red,
                Suit.Hearts => Color.Red,
                Suit.Spades => Color.Black,
                _ => throw new ArgumentOutOfRangeException($"'{Suit}' could not be parsed to a Color.")
            };

        public static implicit operator Card(string card)
        {
            var (cardObj, errorMessage) = Create(card);
            if(cardObj==null)
                throw new ArgumentException(errorMessage);
            else
                return cardObj.Value;
        }

        public static bool operator ==(Card left, Card right) => left.Rank == right.Rank && left.Suit == right.Suit;

        public static bool operator !=(Card left, Card right) => !(left==right);

        public override string ToString()
        {
            var suitChar = Suit switch
            {
                Suit.Clubs => 'C',
                Suit.Diamonds => 'D',
                Suit.Hearts => 'H',
                Suit.Spades => 'S',
                _ => throw new ArgumentOutOfRangeException()
            };
            var rankString = Rank switch
            {
                1 => "A",
                11 => "J",
                12 => "Q",
                13 => "K",
                _ => Rank.ToString()
            };
            return $"{rankString}{suitChar}";
        }

        private static Regex CardRegex = new Regex(@"^(A|1|2|3|4|5|6|7|8|9|10|J|Q|K)(C|D|H|S)$", RegexOptions.IgnoreCase);

        public static (Card? card, string errorMessage) Create(string str)
        {
            var result = CardRegex.Match(str ?? "");
            if (result.Success == false)
                return (null, $"'{str}' is not recognized as a card.");

            var rankString = result.Groups[1].Value;
            var suitString = result.Groups[2].Value;

            Suit suit = suitString.ToUpper() switch
            {
                "C" => Suit.Clubs,
                "D" => Suit.Diamonds,
                "H" => Suit.Hearts,
                "S" => Suit.Spades,
                _ => throw new ArgumentOutOfRangeException($"'{suitString}' could not be parsed to a Suit.")
            };

            int rank = rankString.ToUpper() switch
            {
                "A" => 1,
                "2" => 2,
                "3" => 3,
                "4" => 4,
                "5" => 5,
                "6" => 6,
                "7" => 7,
                "8" => 8,
                "9" => 9,
                "10" => 10,
                "J" => 11,
                "Q" => 12,
                "K" => 13,
                _ => throw new ArgumentOutOfRangeException($"'{rankString}' could not be parsed to a rank.")
            };

            return (new Card(suit, rank), null);
        }
    }
}