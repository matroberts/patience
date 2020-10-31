using System;
using System.Collections.Generic;
using System.Linq;

namespace patience.core
{
    public class Layout
    {
        public Stock Stock { get; set; } = new Stock();
    }

    public class Stock
    {
        public List<Card> Cards { get; set; } = new List<Card>();
        public int Position { get; set; } = 0; // 1-indexed !!
    }


    public static class LayoutExtensions
    {
        public static ApiLayout ToApiLayout(this Layout layout)
        {
            return new ApiLayout(){ Stock = layout.Stock.Cards.Where((c,i) => i+3 == layout.Stock.Position || i+2 == layout.Stock.Position || i+1 == layout.Stock.Position).Select(c => c.ToString()).ToList()};
        }
    }
}