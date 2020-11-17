using System;
using System.Collections.Generic;
using System.Linq;
using patience.core;

namespace patience.console
{
    public static class LayoutRenderer
    {
        public static void Render(this ApiResult result)
        {
            result.Layout.Render();
            Console.WriteLine();
            Console.ForegroundColor = result.Status == ApiStatus.Ok ? ConsoleColor.Black : ConsoleColor.Red;
            Console.WriteLine(result.Message);
        }        
        
        public static void Render(this ApiLayout layout)
        {
            layout.Stock.RenderStock();
            layout.Foundation.RenderFoundation();
            layout.Tableau.RenderTableau();
        }

        public static void RenderStock(this List<string> stock)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            foreach (var stockCard in stock)
            {
                stockCard.Render();
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        public static void RenderFoundation(this List<string> foundation)
        {
            foreach (var foundationCard in foundation)
            {
                foundationCard.Render();
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        public static void RenderTableau(this Dictionary<string, List<string>> tableau)
        {
            // have to flip the direction of the cards from rows to columns
            // find the longest row first
            var longestRow = tableau.Max(kvp => kvp.Value.Count);
            for (int i = 0; i < longestRow; i++)
            {
                foreach (var key in tableau.Keys)
                {
                    var stack = tableau[key];
                    var tableauCard = i >= stack.Count ? "   " : stack[i].ToString();
                    tableauCard.Render();
                }
                Console.WriteLine();
            }
        }

        public static void Render(this string card)
        {
            if (card.EndsWith('D') || card.EndsWith('H'))
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.Black;

            var displayCard = card.Replace('C', '\u2663').Replace('D', '\u2666').Replace('H', '\u2665').Replace('S', '\u2660');
            Console.Write(displayCard + " ");
        }
    }

}