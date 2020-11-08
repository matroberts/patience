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

    public class MoveCommand : ICommand
    {
        public string From { get; set; }
        public string To { get; set; }
        public void Do(Layout layout)
        {
            throw new System.NotImplementedException();
        }

        public void Undo(Layout layout)
        {
            throw new System.NotImplementedException();
        }
    }
}