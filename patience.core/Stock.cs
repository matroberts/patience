using System;
using System.Collections.Generic;

namespace patience.core
{
    public class Stock : IStack
    {
        public string Name => "Stock";

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

        public (int from, int to) Measure()
        {
            int newPosition;
            if (Position == Cards.Count)
                newPosition = 0;
            else
                newPosition = Position+3;

            if (newPosition > Cards.Count)
                newPosition = Cards.Count;
            return (Position, newPosition);
        }

        public void Step(in int from, in int to)
        {
            if(from != Position)
                throw new ArgumentException($"Cannot Step from '{from}' since the current position is '{Position}'");
            Position = to;
            AssertInvariants();
        }

        public bool IsAvailable(Card card)
        {
            if (Position <= 0)
                return false;
            return Cards[Position - 1] == card;
        }

        public Card Take()
        {
            if(Position <= 0)
                throw new ArgumentException("The stock has no card to take.");

            var card = Cards[Position - 1];
            Cards.RemoveAt(Position-1);
            Position--;
            return card;
        }

        public bool CanAccept(Card card) => false;

        public void Give(Card card)
        {
            Cards.Insert(Position, card);
            Position++;
        }

        public void FlipTopCard() => throw new InvalidOperationException("You cannot flip a card in the Stock");
    }
}