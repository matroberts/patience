using System;
using System.Collections.Generic;
using System.Linq;

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
                throw new InvalidOperationException($"Invariant Violation - {Name} Position {Position} is less than 0.");
            if (Position > Cards.Count)
                throw new InvalidOperationException($"Invariant Violation - {Name} Position {Position} is greater than the {Name} count {Cards.Count}.");
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

        public (string stack, bool flipTopCard) IsAvailable(Card card)
        {
            if (Position <= 0)
                return (null, false);
            return (Cards[Position - 1] == card ? Name : null, false);
        }

        public bool CanAccept(Card card) => false;

        public void Give(List<Card> cards)
        {
            // TODO n<>1
            Cards.Insert(Position, cards.First());
            Position++;
        }

        public List<Card> Take(int n)
        {
            // TODO n<>1
            if(Position <= 0)
                throw new ArgumentException($"The {Name} has no card to take.");

            var card = Cards[Position - 1];
            Cards.RemoveAt(Position-1);
            Position--;
            return new List<Card>(){ card };
        }

        public void FlipTopCard() => throw new InvalidOperationException($"You cannot flip a card in the {Name}");
    }
}