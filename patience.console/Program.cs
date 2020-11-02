using System;
using patience.core;

namespace patience.console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, eventArgs) => Exit();
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

            IKlondike klondike = new Klondike(new Layout()
            {
                Stock = new Stock() { Cards = { "AC", "2C", "3C", "4H", "5D", "6S", "7C" }, Position = 0 } // last position
            });

            Console.Clear();
            Console.WriteLine("Press H for help.");
            while (true)
            {
                Console.Write("Patience> ");
                var operation = Console.ReadLine();
                Console.Clear();

                if (operation.StartsWith("E", StringComparison.OrdinalIgnoreCase))
                {
                    Exit();
                } 
                else if (operation.StartsWith("H", StringComparison.OrdinalIgnoreCase))
                {
                    Help();
                    continue;
                }

                var result = klondike.Operate(operation);

                if (result.Status != ApiStatus.Ok)
                {
                    WriteError(result.ErrorMessage);
                }

                result.Layout.Render();
            }
        }

        private static void Exit()
        {
            Console.ResetColor();
            Console.Clear();
            Environment.Exit(0);
        }

        private static void Help()
        {
            Console.WriteLine("  H Help");
            Console.WriteLine("  E Exit");
            Console.WriteLine("  D Deal - turn over 3 cards from the stock");
            Console.WriteLine("  S Show - print the cards out without making any move");
        }

        public static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Black;
        }
    }
}
