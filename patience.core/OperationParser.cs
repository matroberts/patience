using System;
using System.Windows.Input;

namespace patience.core
{
    public class OperationParser
    {
        public (Act act, ICommand command, string errorMessage) Parse(Layout layout, string apiOperation)
        {
            if (apiOperation.StartsWith("H", StringComparison.OrdinalIgnoreCase))
                return (Act.Help, null, null);
            else if (apiOperation.StartsWith("D", StringComparison.OrdinalIgnoreCase))
                return (Act.Do, new DealCommand(), null);
            else if (apiOperation.StartsWith("F", StringComparison.OrdinalIgnoreCase))
            {
                var (command, errorMessage) = MakeMove(layout, apiOperation);
                if(command == null)
                    return (Act.Error, null, errorMessage);
                else
                    return (Act.Do, command, null);
            }
            else
                return (Act.Error, null, $"Operation '{apiOperation}' is not understood.");
        }

        private (MoveCommand command, string errorMessage) MakeMove(Layout layout, string apiOperation)
        {
            string target = apiOperation.Substring(0, 1);
            string cardStr = apiOperation.Substring(1, apiOperation.Length - 1);

            var (cardNullable, errorMessage) = Card.Create(cardStr);
            if (cardNullable == null)
                return (null, errorMessage);
            var card = cardNullable.Value;

            return (new MoveCommand(), null);
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