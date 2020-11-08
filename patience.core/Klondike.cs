using System;

namespace patience.core
{
    public class Klondike : IKlondike
    {
        private readonly Layout layout;
        private readonly OperationParser parser;

        public Klondike(Layout layout)
        {
            this.parser = new OperationParser();
            this.layout = layout;
            this.layout.AssertInvariants();
        }

        public ApiResult Operate(string apiOperation)
        {
            var (act, command, errorMessage) = parser.Parse(layout, apiOperation);

            switch (act)
            {
                case Act.Do:
                    command.Do(layout);
                    return new ApiResult()
                    {
                        Status = ApiStatus.Ok,
                        Layout = layout.ToApiLayout()
                    };
                case Act.Undo:
                    throw new NotImplementedException();
                case Act.Redo:
                    throw new NotImplementedException();
                case Act.Help:
                    return new ApiResult()
                    {
                        Message = @"
Ctrl+C Exit
H      Help
D      Deal - turn over 3 cards from the stock",
                        Status = ApiStatus.Ok,
                        Layout = layout.ToApiLayout()
                    };
                case Act.Error:
                    return new ApiResult()
                    {
                        Message = errorMessage,
                        Status = ApiStatus.OperationNotUnderstood,
                        Layout = layout.ToApiLayout()
                    };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}