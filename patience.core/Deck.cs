using System.Collections.Generic;

namespace patience.core
{
    public class Deck
    {
        public List<Card> Create()
        {
            var cards = new List<Card>();
            for (int i=0; i < 52; i++)
            {
                Suit suit = (Suit)(i / 13);
                int rank = i % 13 + 1;
                cards.Add(new Card(suit, rank));
            }
            return cards;
        }
    }
}