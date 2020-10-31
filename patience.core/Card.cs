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

        public static implicit operator Card(string card)
        {
            if(card.Length <= 1)
                throw new ArgumentException("String to short in implicit cast to Card");
            if(card.Length > 3)
                throw new ArgumentException("String to long in implicit cast to Card");

            var suitChar = card[^1];
            var suit = suitChar switch
            {
                'C' => Suit.Clubs,
                'D' => Suit.Diamonds,
                'H' => Suit.Hearts,
                'S' => Suit.Spades,
                _ => throw new ArgumentException($"Suit '{suitChar}' is not recognized.  Allowed values are C,D,H,S.")
            };

            var rankString = card.Substring(0, card.Length - 1);
            var rank = rankString switch
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
                _ => throw new ArgumentException($"Rank '{rankString}' is not recognized.  Allowed values are A,2,3,4,5,6,7,8,9,10,J,Q,K.")
            };
            return new Card(suit, rank);
        }

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
    }
}