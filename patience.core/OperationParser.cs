namespace patience.core
{
    public interface IOperation
    {
    }

    public class PrintOperation : IOperation
    {

    }

    public class DealOperation : IOperation
    {

    }

    public class OperationParser
    {
        public (IOperation operation, string errorMessage) Parse(string apiOperation)
        {
            if (apiOperation == "P")
                return (new PrintOperation(), null);
            else if (apiOperation == "D")
                return (new DealOperation(), null);
            else
                return (null, $"Operation '{apiOperation}' is not understood.  Allowed operations are P,D,U,R,<card>T, <card>TT, <card>F.");
        }
    }
}