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


        /// <summary>
        /// Measure and Step
        /// To support undo, when you deal the deck you need to record the before and after position of the stock
        /// That way un-deal can just be the reverse of the positions
        /// Un-deal is not simply Position-=3 because at the end of the deck you could have come from three possible positions
        /// </summary>
        public (int from, int to) Measure() => Stock.Measure();

        public void Step(int from, int to) => Stock.Step(from, to);

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

        public string CanFoundationAccept(Card card)
        {
            return Foundation.Stacks.FirstOrDefault(s => s.CanAccept(card))?.Name;
        }
    }

    public class Foundation
    {
        public FoundationStack ClubsStack { get; set; } = new FoundationStack(Suit.Clubs);
        public FoundationStack DiamondsStack { get; set; } = new FoundationStack(Suit.Diamonds);
        public FoundationStack HeartsStack { get; set; } = new FoundationStack(Suit.Hearts);
        public FoundationStack SpadesStack { get; set; } = new FoundationStack(Suit.Spades);

        public IEnumerable<FoundationStack> Stacks
        {
            get
            {
                yield return ClubsStack;
                yield return DiamondsStack;
                yield return HeartsStack;
                yield return SpadesStack;
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
                Foundation = layout.Foundation.ToApiFoundation(),
            };
        }

        public static List<string> ToApiStock(this Stock stock)
        {
            var list = new List<string>();
            list.Add(stock.MoreStock ? "XX" : "--");
            list.AddRange(stock.Cards.Where((c, i) => i + 3 == stock.Position || i + 2 == stock.Position || i + 1 == stock.Position).Select(c => c.ToString()));
            return list;
        }

        public static List<string> ToApiFoundation(this Foundation foundation) => foundation.Stacks.Select(stack => stack.Cards.Any() ? stack.Cards.Last().ToString() : "--").ToList();
    }
}