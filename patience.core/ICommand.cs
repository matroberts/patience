namespace patience.core
{
    public interface ICommand
    {
        void Do(Layout layout);
        void Undo(Layout layout);
    }

    public class DealCommand : ICommand
    {
        public void Do(Layout layout)
        {
            layout.Deal();
        }

        public void Undo(Layout layout)
        {
            throw new System.NotImplementedException();
        }
    }
}