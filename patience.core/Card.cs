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
            var (cardObj, errorMessage) = Create(card);
            if(cardObj==null)
                throw new ArgumentException(errorMessage);
            else
                return cardObj.Value;
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

        public static (Card? card, string errorMessage) Create(string card)
        {
            if (card.Length <= 1)
                return (null, $"'{card}' is to short to be a card, you need to specify rank and suit, like 4C.");
            if (card.Length > 3)
                return (null, $"'{card}' is to long to be a card, you need to specify rank and suit, like 4C.");

            var suitChar = card[^1];

            Suit suit;
            switch (char.ToUpper(suitChar))
            {
                case 'C':
                    suit = Suit.Clubs;
                    break;
                case 'D':
                    suit = Suit.Diamonds;
                    break;
                case 'H':
                    suit = Suit.Hearts;
                    break;
                case 'S':
                    suit = Suit.Spades;
                    break;
                default:
                    return (null, $"Suit '{suitChar}' is not recognized.  Allowed values are C,D,H,S.");
            }

            var rankString = card.Substring(0, card.Length - 1);
            int rank;
            switch (rankString.ToUpper())
            {
                case "A":
                    rank = 1;
                    break;
                case "2":
                    rank = 2;
                    break;
                case "3":
                    rank = 3;
                    break;
                case "4":
                    rank = 4;
                    break;
                case "5":
                    rank = 5;
                    break;
                case "6":
                    rank = 6;
                    break;
                case "7":
                    rank = 7;
                    break;
                case "8":
                    rank = 8;
                    break;
                case "9":
                    rank = 9;
                    break;
                case "10":
                    rank = 10;
                    break;
                case "J":
                    rank = 11;
                    break;
                case "Q":
                    rank = 12;
                    break;
                case "K":
                    rank = 13;
                    break;
                default:
                    return (null, $"Rank '{rankString}' is not recognized.  Allowed values are A,2,3,4,5,6,7,8,9,10,J,Q,K.");
            }

            return (new Card(suit, rank), null);
        }
    }
}