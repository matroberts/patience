using System;

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
                    Message = errorMessage,
                    Status = ApiStatus.OperationNotUnderstood,
                    Layout = layout.ToApiLayout()
                };
            } 
            if (operation is OperationHelp)
            {
                return new ApiResult()
                {
                    Message = @"
Ctrl+C Exit
H      Help
D      Deal - turn over 3 cards from the stock",
                    Status = ApiStatus.Ok,
                    Layout = layout.ToApiLayout()
                };
            }

            if (operation is OperationDeal)
            {
                layout.Deal();
            }

            return new ApiResult()
            {
                Status = ApiStatus.Ok,
                Layout = layout.ToApiLayout()
            };
        }
    }
}