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
            // parse the operation - turn this into a parsed action

            // validation the operation - actually makes a move to apply to the layout

            // do the operation - do the move to the layout

            return new ApiResult()
            {
                Status = ApiStatus.Ok,
                Layout = layout.ToApiLayout()
            };
        }
    }
}