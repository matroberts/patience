using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace patience.core
{
    public class Layout
    {
        public Stock Stock { get; } = new Stock();
        public Foundation Foundation { get; set; } = new Foundation();

        public void AssertInvariants()
        {
            Stock.AssertInvariants();
            Foundation.AssertInvariants();
        }

        public void Deal() => Stock.Deal();
    }

    public class Foundation
    {
        public FoundationStack ClubStack { get; set; } = new FoundationStack(Suit.Clubs);
        public FoundationStack DiamondStack { get; set; } = new FoundationStack(Suit.Diamonds);
        public FoundationStack HeartStack { get; set; } = new FoundationStack(Suit.Hearts);
        public FoundationStack SpadeStack { get; set; } = new FoundationStack(Suit.Spades);

        public IEnumerable<FoundationStack> Stacks
        {
            get
            {
                yield return ClubStack;
                yield return DiamondStack;
                yield return HeartStack;
                yield return SpadeStack;
            }
        }

        public void AssertInvariants()
        {
            foreach (var stack in Stacks)
            {
                stack.AssertInvariants();
            }
        }
    }

    public class FoundationStack : IEnumerable<Card>
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
                throw new InvalidOperationException($"Invariant Violation - FoundationStack {Suit} contains the card '{Cards.First(c => c.Suit != Suit)}' which does not match suit.");
            if (Cards.Where( (c, i) => c.Rank != i+1).Any())
                throw new InvalidOperationException($"Invariant Violation - FoundationStack {Suit} is not in rank order, ranks are '{string.Join(", ", Cards.Select(c => c.Rank.ToString()))}'.");
        }
    }

    public class Stock
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
    }

    public static class LayoutExtensions
    {
        public static ApiLayout ToApiLayout(this Layout layout)
        {
            return new ApiLayout()
            {
                Stock = layout.Stock.ToApiStock(),
                MoreStock = layout.Stock.MoreStock,
                Foundation = layout.Foundation.ToApiFoundation(),
            };
        }

        public static List<string> ToApiStock(this Stock stock) => stock.Cards.Where((c, i) => i + 3 == stock.Position || i + 2 == stock.Position || i + 1 == stock.Position).Select(c => c.ToString()).ToList();
        public static List<string> ToApiFoundation(this Foundation foundation) => (from stack in foundation.Stacks where stack.Cards.Any() select stack.Cards.Last().ToString()).ToList();
    }
}