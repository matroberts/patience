﻿namespace patience.core
{
    public interface ICommand
    {
        void Do(Layout layout);
        void Undo(Layout layout);
    }

    public class DealCommand : ICommand
    {
        public int From { get; set; }
        public int To { get; set; }
        public void Do(Layout layout)
        {
            layout.Step(From, To);
        }

        public void Undo(Layout layout)
        {
            layout.Step(To, From);
        }
    }

    public class MoveCommand : ICommand
    {
        public bool FlipTopCard { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public void Do(Layout layout)
        {
            layout.Move(From, 1, To);
            if(FlipTopCard)
                layout.FlipTopCard(From);
        }

        public void Undo(Layout layout)
        {
            if (FlipTopCard)
                layout.FlipTopCard(From);
            layout.Move(To, 1, From);
        }
    }
}