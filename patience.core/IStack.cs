using System.Collections.Generic;

namespace patience.core
{
    public interface IStack
    {
        public string Name { get; }
        public bool IsAvailable(Card card);
        Card Take();
        public bool CanAccept(Card card);
        void Give(Card card);
        void AssertInvariants();
        public List<Card> Cards { get; }

    }
}