using System;
using System.Collections.Generic;

namespace patience.core
{
    public class Stock : IStack
    {
        public List<Card> Cards { get; } = new List<Card>();
        /// <summary>
        /// Stock position is 1-indexed.  A zero indicates that no cards have been flipped yet.
        /// </summary>
        public int Position { get; set; } = 0;

        public bool MoreStock => Position < Cards.Count;

        public void AssertInvariants()
        {
            if(Position < 0)
                throw new InvalidOperationException($"Invariant Violation - Stock Position {Position} is less than 0.");
            if (Position > Cards.Count)
                throw new InvalidOperationException($"Invariant Violation - Stock Position {Position} is greater than the stock count {Cards.Count}.");
        }

        public void Deal()
        {
            if (Position == Cards.Count)
                Position = 0;
            else
                Position += 3;

            if (Position > Cards.Count)
                Position = Cards.Count;
        }

        public string Name => "Stock";
        public Card Take()
        {
            if(Position <= 0)
                throw new ArgumentException("The stock has no card to take.");

            var card = Cards[Position - 1];
            Cards.RemoveAt(Position-1);
            Position--;
            return card;
        }

        public void Give(Card card)
        {
            throw new NotImplementedException();
        }
    }
}