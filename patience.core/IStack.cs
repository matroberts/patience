using System.Collections.Generic;
using Newtonsoft.Json.Bson;

namespace patience.core
{
    public interface IStack
    {
        public string Name { get; }
        void AssertInvariants();
        public (string stack, bool flipTopCard) IsAvailable(Card card);
        public bool CanAccept(Card card);
        void Give(List<Card> cards);
        List<Card> Take(int n);
        void FlipTopCard();
        public List<Card> Cards { get; }
    }
}