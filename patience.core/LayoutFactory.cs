using System.Collections.Generic;

namespace patience.core
{
    public class LayoutFactory
    {
        public List<Card> CreateDeck()
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

        //public Layout C
    }
}