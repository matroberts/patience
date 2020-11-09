using System;
using System.Collections.Generic;
using System.Linq;

namespace patience.core
{
    public class Layout
    {
        public Stock Stock { get; } = new Stock();
        public Foundation Foundation { get; set; } = new Foundation();

        public IEnumerable<IStack> Stacks
        {
            get
            {
                yield return Stock;
                foreach (var stack in Foundation.Stacks)
                    yield return stack;
            }
        }

        public void AssertInvariants()
        {
            foreach (var stack in Stacks)
            {
                stack.AssertInvariants();
            }
        }

        public void Deal() => Stock.Deal();

        public void Move(string from, string to)
        {
            var fromStack = Stacks.FirstOrDefault(s => s.Name == from);
            if(fromStack == null)
                throw new ArgumentException($"From stack '{from}' does not exist.");

            var toStack = Stacks.FirstOrDefault(s => s.Name == to);
            if (toStack == null)
                throw new ArgumentException($"To stack '{to}' does not exist.");

            var card = fromStack.Take();
            toStack.Give(card);
        }

        public string IsAvailable(Card card)
        {
            return Stacks.FirstOrDefault(s => s.IsAvailable(card))?.Name;
        }
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
        public static List<string> ToApiFoundation(this Foundation foundation) => foundation.Stacks.Select(stack => stack.Cards.Any() ? stack.Cards.Last().ToString() : "--").ToList();
    }
}