using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace patience.core
{
    public class FoundationStack : IEnumerable<Card>, IStack
    {
        public string Name => $"{this.Suit}Stack";

        public FoundationStack(Suit suit) => Suit = suit;
        public Suit Suit { get; }
        public List<Card> Cards { get; set; } = new List<Card>();
        public IEnumerator<Card> GetEnumerator() => Cards.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public void Add(Card card) => Cards.Add(card);

        public void AssertInvariants()
        {
            if (Cards.Any(c => c.Suit != Suit))
                throw new InvalidOperationException($"Invariant Violation - {Name} contains the card '{Cards.First(c => c.Suit != Suit)}' which does not match suit.");
            if (Cards.Where( (c, i) => c.Rank != i+1).Any())
                throw new InvalidOperationException($"Invariant Violation - {Name} is not in rank order, ranks are '{string.Join(", ", Cards.Select(c => c.Rank.ToString()))}'.");
        }

        public bool IsAvailable(Card card)
        {
            if (Cards.Any() == false)
                return false;
            return Cards.Last() == card;
        }

        public Card Take()
        {
            if (Cards.Count == 0)
                throw new ArgumentException($"The {Name} has no card to take.");

            var card = Cards[^1];
            Cards.RemoveAt(Cards.Count-1);
            return card;
        }

        public bool CanAccept(Card card)
        {
            if (Suit != card.Suit)
                return false;
            return Cards.Count + 1 == card.Rank;
        }

        public void Give(Card card)
        {
            Cards.Add(card);
            AssertInvariants();
        }

        public void FlipTopCard() => throw new InvalidOperationException($"You cannot flip a card in the {Name}");
    }
}