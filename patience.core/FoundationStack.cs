using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace patience.core
{
    public class FoundationStack : IEnumerable<Card>, IStack
    {
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

        public string Name => $"{this.Suit}Stack";

        public bool IsAvailable(Card card)
        {
            if (Cards.Any() == false)
                return false;
            return Cards.Last() == card;
        }

        public Card Take()
        {
            throw new NotImplementedException();
        }

        public bool CanAccept(Card card)
        {
            if (Suit != card.Suit)
                return false;
            return Cards.Count + 1 == card.Rank;
        }

        public void Give(Card card)
        {
            if(card.Suit != Suit)
                throw new ArgumentException($"Cannot give card '{card}' to the {Name} because the suit is wrong.");
            if(card.Rank != Cards.Count+1)
                throw new ArgumentException($"Cannot give card '{card}' to the {Name} because the rank is wrong.");
            Cards.Add(card);
        }
    }
}