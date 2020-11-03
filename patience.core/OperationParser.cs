using System;

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
    }
}