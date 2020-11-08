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
            else
                return (Act.Error, null, $"Operation '{apiOperation}' is not understood.");
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