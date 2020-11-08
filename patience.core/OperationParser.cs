using System;
using System.Windows.Input;

namespace patience.core
{
    public interface IOperation
    {
    }

    public class OperationHelp : IOperation
    {

    }

    public class OperationDeal : IOperation
    {

    }

    public class OperationParser
    {
        public (IOperation operation, string errorMessage) Parse(string apiOperation)
        {
            if (apiOperation.StartsWith("H", StringComparison.OrdinalIgnoreCase))
                return (new OperationHelp(), null);
            else if (apiOperation.StartsWith("D", StringComparison.OrdinalIgnoreCase))
                return (new OperationDeal(), null);
            else
                return (null, $"Operation '{apiOperation}' is not understood.");
        }


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