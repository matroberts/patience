using System;
using patience.core;

namespace patience.console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();

            IKlondike klondike = new Klondike(new Layout()
            {
                Stock = new Stock() { Cards = { "AC", "2C", "3C", "4H", "5D", "6S", "7C" }, Position = 0 } // last position
            });

            while (true)
            {
                Console.Write("Patience>");
                var operation = Console.ReadLine();
                var result = klondike.Operate(operation);

                if (result.Status != ApiStatus.Ok)
                {
                    WriteError(result.ErrorMessage);
                }

                result.Layout.Render();
            }
        }

        public static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Black;
        }
    }
}
