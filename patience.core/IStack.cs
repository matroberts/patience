using System.Collections.Generic;
using Newtonsoft.Json.Bson;

namespace patience.core
{
    public interface IStack
    {
        public string Name { get; }
        public (string stack, bool flipTopCard) IsAvailable(Card card);
        Card Take();
        public bool CanAccept(Card card);
        void Give(Card card);
        void FlipTopCard();
        void AssertInvariants();
        public List<Card> Cards { get; }

    }
}