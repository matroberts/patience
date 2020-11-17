using System;
using System.Collections.Generic;
using System.Linq;

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

        public Layout CreateLayout(List<Card> cards)
        {
            if(cards.Count != 52)
                throw new ArgumentException("You must give CreateLayout a full deck of cards");

            var layout = new Layout();
            layout.Tableau.T1Stack.Cards.AddRange(cards.Skip(0).Take(1)); 
            layout.Tableau.T1Stack.FlippedAt = 1;
            layout.Tableau.T2Stack.Cards.AddRange(cards.Skip(1).Take(2));
            layout.Tableau.T2Stack.FlippedAt = 2;
            layout.Tableau.T3Stack.Cards.AddRange(cards.Skip(3).Take(3));
            layout.Tableau.T3Stack.FlippedAt = 3;
            layout.Tableau.T4Stack.Cards.AddRange(cards.Skip(6).Take(4));
            layout.Tableau.T4Stack.FlippedAt = 4;
            layout.Tableau.T5Stack.Cards.AddRange(cards.Skip(10).Take(5));
            layout.Tableau.T5Stack.FlippedAt = 5;
            layout.Tableau.T6Stack.Cards.AddRange(cards.Skip(15).Take(6));
            layout.Tableau.T6Stack.FlippedAt = 6;
            layout.Tableau.T7Stack.Cards.AddRange(cards.Skip(21).Take(7));
            layout.Tableau.T7Stack.FlippedAt = 7;
            layout.Stock.Cards.AddRange(cards.Skip(28).Take(24));
            return layout;
        }

        public Layout Create()
        {
            var deck = CreateDeck();
            deck.Shuffle();
            return CreateLayout(deck);
        }
    }
}