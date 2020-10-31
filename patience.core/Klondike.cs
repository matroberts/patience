namespace patience.core
{
    public class Klondike
    {
        private readonly InternalLayout internalLayout;

        public Klondike(InternalLayout internalLayout)
        {
            this.internalLayout = internalLayout;
        }

        public Result Operate(string operation)
        {
            // parse the operation 

            // validation the operation

            // do the operation

            // map internal layout to layout
            return new Result(){Status = Status.Ok};
        }
    }
}