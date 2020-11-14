using System;
using System.Collections.Generic;

namespace patience.core
{
    public class Klondike : IKlondike
    {
        private readonly Layout layout;
        private readonly OperationParser parser;
        private readonly Stack<ICommand> history = new Stack<ICommand>();

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
                case Act.Error:
                    return new ApiResult() { Message = errorMessage, Status = ApiStatus.Error, Layout = layout.ToApiLayout() };
                case Act.Help:
                    return new ApiResult() { Message = helpMessage, Status = ApiStatus.Ok, Layout = layout.ToApiLayout() };
                case Act.Do:
                    history.Push(command);
                    command.Do(layout);
                    break;
                case Act.Undo:
                    if(history.Count > 0)
                        history.Pop().Undo(layout);
                    break;
                case Act.Redo:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new ApiResult()
            {
                Status = ApiStatus.Ok,
                Layout = layout.ToApiLayout()
            };
        }

        private readonly string helpMessage = @"
Ctrl+C  Exit
H       Help
D       Deal - turn over 3 cards from the stock
U       Undo
F<card> Move the <card> to the Foundation
<card>  Examples: AC, 3D, JH, KS";
    }
}