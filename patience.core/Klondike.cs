namespace patience.core
{
    public class Klondike : IKlondike
    {
        private readonly Layout layout;

        public Klondike(Layout layout)
        {
            this.layout = layout;
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