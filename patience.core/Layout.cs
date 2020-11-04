using System;
using System.Collections.Generic;
using System.Linq;

namespace patience.core
{
    public class Layout
    {
        public Stock Stock { get; } = new Stock();
        public Foundation Foundation { get; set; } = new Foundation();

        public void Deal() => Stock.Deal();
    }

    public class Foundation
    {
        public FoundationStack ClubStack { get; set; } = new FoundationStack(Suit.Clubs);
        public FoundationStack DiamondStack { get; set; } = new FoundationStack(Suit.Diamonds);
        public FoundationStack HeartStack { get; set; } = new FoundationStack(Suit.Hearts);
        public FoundationStack SpadeStack { get; set; } = new FoundationStack(Suit.Spades);
    }

    public class FoundationStack
    {
        public FoundationStack(Suit suit) => Suit = suit;
        public Suit Suit { get; }
        public List<Card> Cards { get; set; } = new List<Card>();
    }

    public class Stock
    {
        public List<Card> Cards { get; set; } = new List<Card>();
        /// <summary>
        /// Stock position is 1-indexed.  A zero indicates that no cards have been flipped yet.
        /// </summary>
        public int Position { get; set; } = 0;

        public bool MoreStock => Position < Cards.Count;

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
            };
        }

        public static List<string> ToApiStock(this Stock stock) => stock.Cards.Where((c, i) => i + 3 == stock.Position || i + 2 == stock.Position || i + 1 == stock.Position).Select(c => c.ToString()).ToList();
    }
}