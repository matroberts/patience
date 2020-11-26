using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace patience.core
{
    public class TableauStack : IEnumerable<Card>, IStack
    {
        public TableauStack(string label) => this.label = label;

        private readonly string label;
        public string Name => $"{this.label}Stack";

        public List<Card> Cards { get; set; } = new List<Card>();
        public IEnumerator<Card> GetEnumerator() => Cards.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public void Add(Card card) => Cards.Add(card);

        /// <summary>
        /// TableauStack FlippedAt is 1-indexed.  The FlippedAt is the first card which is face up.
        /// </summary>
        public int FlippedAt { get; set; } = 0;

        private IEnumerable<Card> FlippedCards => Cards.Where((c, i) => i + 1 >= FlippedAt);

        public void AssertInvariants()
        {
            foreach(var (first, second) in FlippedCards.Zip(FlippedCards.Skip(1), (first, second) => (first, second)))
            {
                if (first.Rank != second.Rank+1)
                    throw new InvalidOperationException($"Invariant Violation - {Name} flipped cards are not in descending order.  Flipped cards are: {string.Join(", ", FlippedCards.Select(c => c.ToString()))}.");
                if (first.Color == second.Color)
                    throw new InvalidOperationException($"Invariant Violation - {Name} flipped cards are not alternating color.  Flipped cards are: {string.Join(", ", FlippedCards.Select(c => c.ToString()))}.");
            }

            if (FlippedAt > Cards.Count + 1)
                throw new InvalidOperationException($"Invariant Violation - {Name} flipped card position is too far beyond the end of the stack.  FlippedAt {FlippedAt} NumberCards {Cards.Count}.");
            if (FlippedAt < 0)
                throw new InvalidOperationException($"Invariant Violation - {Name} flipped card position is less than zero.");
        }

        public bool IsAvailable(Card card)
        {
            if(Cards.Count == 0)
                return false;
            return Cards.Last() == card;
        }

        public Card Take()
        {
            if (Cards.Count == 0)
                throw new ArgumentException($"The {Name} has no card to take.");

            var card = Cards[^1];
            Cards.RemoveAt(Cards.Count - 1);
            return card;
        }

        public bool CanAccept(Card card)
        {
            if (Cards.Count == 0)
                return card.Rank == 13;  // king rule

            return Cards.Last().Color != card.Color && Cards.Last().Rank == card.Rank + 1;
        }

        public void Give(Card card)
        {
            Cards.Add(card);
            AssertInvariants();
        }

        public void FlipTopCard()
        {
            if (FlippedAt == Cards.Count)
                FlippedAt++;
            else if (FlippedAt == Cards.Count + 1)
                FlippedAt--;
            else
                throw new InvalidOperationException($"The {Name} is in the wrong position for FlipTopCard.  FlippedAt={FlippedAt} NumberCards={Cards.Count}.");
        }
    }
}