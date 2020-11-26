using System;

namespace patience.core
{
    public class OperationParser
    {
        public (Act act, ICommand command, string errorMessage) Parse(Layout layout, string apiOperation)
        {
            if (apiOperation.StartsWith("H", StringComparison.OrdinalIgnoreCase))
                return (Act.Help, null, null);
            else if (apiOperation.StartsWith("U", StringComparison.OrdinalIgnoreCase))
            {
                return (Act.Undo, null, null);
            }
            else if (apiOperation.StartsWith("D", StringComparison.OrdinalIgnoreCase))
            {
                return (Act.Do, MakeDeal(layout), null);
            }
            else if (Card.Create(apiOperation).card != null)
            {
                var (command, errorMessage) = MakeMove(layout, apiOperation);
                if (command == null)
                    return (Act.Error, null, errorMessage);
                else
                    return (Act.Do, command, null);
            }
            else if (apiOperation.StartsWith("F", StringComparison.OrdinalIgnoreCase))
            {
                var (command, errorMessage) = MakeFoundationMove(layout, apiOperation);
                if(command == null)
                    return (Act.Error, null, errorMessage);
                else
                    return (Act.Do, command, null);
            }
            else
                return (Act.Error, null, $"Operation '{apiOperation}' is not understood.");
        }

        private DealCommand MakeDeal(Layout layout)
        {
            var (from, to) = layout.Measure();
            return new DealCommand(){From = from, To = to};
        }

        private (MoveCommand command, string errorMessage) MakeMove(Layout layout, string apiOperation)
        {
            var card = Card.Create(apiOperation).card.Value;

            var (from, flipTopCard) = layout.IsAvailable(card);
            if (from == null)
                return (null, $"'{card}' is not available to be moved.");

            var to = layout.CanAccept(card);
            if (to == null)
                return (null, $"'{card}' cannot be moved anywhere.");

            return (new MoveCommand() { From = from, To = to, FlipTopCard = flipTopCard}, null);
        }

        private (MoveCommand command, string errorMessage) MakeFoundationMove(Layout layout, string apiOperation)
        {
            string target = apiOperation.Substring(0, 1);
            string cardStr = apiOperation.Substring(1, apiOperation.Length - 1);

            var (cardNullable, errorMessage) = Card.Create(cardStr);
            if (cardNullable == null)
                return (null, errorMessage);
            var card = cardNullable.Value;

            var (from, flipTopCard) = layout.IsAvailable(card);
            if(from==null)
                return (null, $"'{card}' is not available to be moved.");

            var to = layout.CanFoundationAccept(card);
            if (to == null)
                return (null, $"'{card}' cannot be moved to the foundation.");

            return (new MoveCommand(){From = from, To = to, FlipTopCard = flipTopCard}, null);
        }
    }



    public enum Act
    {
        Do,
        Undo,
        Redo,
        Help,
        Error
    }
}