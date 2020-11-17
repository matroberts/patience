using System;
using System.Text;
using patience.core;

namespace patience.console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, eventArgs) => Exit();
            Console.BackgroundColor = ConsoleColor.White;
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "Patience";
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.BufferWidth;

            IKlondike klondike = new Klondike(new LayoutFactory().Create());

            Console.Clear();
            klondike.Operate("H").Render();
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine();
                Console.Write("> ");
                var operation = Console.ReadLine();
                Console.Clear();

                klondike.Operate(operation).Render();
            }
        }

        private static void Exit()
        {
            Console.ResetColor();
            Console.Clear();
            Environment.Exit(0);
        }
    }
}
