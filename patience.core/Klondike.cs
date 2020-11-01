namespace patience.core
{
    public class Klondike : IKlondike
    {
        private readonly Layout layout;
        private readonly OperationParser parser;

        public Klondike(Layout layout)
        {
            this.layout = layout;
            this.parser = new OperationParser();
        }

        public ApiResult Operate(string apiOperation)
        {
            var (operation, errorMessage) = parser.Parse(apiOperation);
            if (operation == null)
            {
                return new ApiResult()
                {
                    ErrorMessage = errorMessage,
                    Status = ApiStatus.OperationNotUnderstood,
                    Layout = layout.ToApiLayout()
                };
            }



            // validation the operation - actually makes a Command to apply to the layout

            // do the operation - do the move to the layout

            return new ApiResult()
            {
                Status = ApiStatus.Ok,
                Layout = layout.ToApiLayout()
            };
        }
    }
}