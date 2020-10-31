using System.Collections.Generic;

namespace patience.core
{
    public class InternalLayout
    {
        public Stock Stock { get; set; } = new Stock();
    }

    public class Stock
    {
        public List<Card> Cards { get; set; } = new List<Card>();
        public int Position { get; set; } = 0;
    }
}