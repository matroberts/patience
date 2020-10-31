namespace patience.core
{
    public class Klondike : IKlondike
    {
        private readonly InternalLayout internalLayout;

        public Klondike(InternalLayout internalLayout)
        {
            this.internalLayout = internalLayout;
        }

        public ApiResult Operate(string operation)
        {
            // parse the operation 

            // validation the operation

            // do the operation

            // map internal layout to layout
            return new ApiResult(){Status = ApiStatus.Ok};
        }
    }
}