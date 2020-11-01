namespace patience.core
{
    public interface IOperation
    {
    }

    public class OperationPrint : IOperation
    {

    }

    public class OperationDeal : IOperation
    {

    }

    public class OperationParser
    {
        public (IOperation operation, string errorMessage) Parse(string apiOperation)
        {
            if (apiOperation == "P")
                return (new OperationPrint(), null);
            else if (apiOperation == "D")
                return (new OperationDeal(), null);
            else
                return (null, $"Operation '{apiOperation}' is not understood.  Allowed operations are P,D,U,R,<card>T, <card>TT, <card>F.");
        }
    }
}