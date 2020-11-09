namespace patience.core
{
    public interface IStack
    {
        public string Name { get; }
        public bool IsAvailable(Card card);
        Card Take();
        void Give(Card card);
        void AssertInvariants();

    }
}