using System.Collections.Generic;

namespace patience.core
{
    public class InternalLayout
    {
        public Stock Stock { get; set; }
    }

    public class Stock
    {
        public List<Card> Cards { get; set; }
        public int Position { get; set; } = 0;
    }
}