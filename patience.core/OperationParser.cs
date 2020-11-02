using System;

namespace patience.core
{
    public interface IOperation
    {
    }

    public class OperationShow : IOperation
    {

    }

    public class OperationDeal : IOperation
    {

    }

    public class OperationParser
    {
        public (IOperation operation, string errorMessage) Parse(string apiOperation)
        {
            if (apiOperation.StartsWith("S", StringComparison.OrdinalIgnoreCase))
                return (new OperationShow(), null);
            else if (apiOperation.StartsWith("D", StringComparison.OrdinalIgnoreCase))
                return (new OperationDeal(), null);
            else
                return (null, $"Operation '{apiOperation}' is not understood.");
        }
    }
}