namespace patience.core
{
    public interface IStack
    {
        public string Name { get; }
        Card Take();
        void Give(Card card);
        void AssertInvariants();
    }
}