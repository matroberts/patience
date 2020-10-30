using System;

namespace patience.core
{
    public enum Suit
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
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


        public override string ToString()
        {
            var suitChar = Suit switch
            {
                Suit.Clubs => '\u2663',
                Suit.Diamonds => '\u2662',
                Suit.Hearts => '\u2661',
                Suit.Spades => '\u2660',
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
    }
}