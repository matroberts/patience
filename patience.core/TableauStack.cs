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
                    throw new InvalidOperationException($"Invariant Violation - T1Stack flipped cards are not in descending order.  Flipped cards are: {string.Join(", ", FlippedCards.Select(c => c.ToString()))}.");
                if (first.Color == second.Color)
                    throw new InvalidOperationException($"Invariant Violation - T1Stack flipped cards are not alternating color.  Flipped cards are: {string.Join(", ", FlippedCards.Select(c => c.ToString()))}.");
            }
        }

        public bool IsAvailable(Card card)
        {
            throw new System.NotImplementedException();
        }

        public Card Take()
        {
            throw new System.NotImplementedException();
        }

        public bool CanAccept(Card card)
        {
            throw new System.NotImplementedException();
        }

        public void Give(Card card)
        {
            throw new System.NotImplementedException();
        }


    }
}