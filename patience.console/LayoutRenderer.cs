﻿using System;
using patience.core;

namespace patience.console
{
    public static class LayoutRenderer
    {
        public static void Render(this ApiLayout layout)
        {
            foreach (var stockCard in layout.Stock)
            {
                stockCard.Render();
                Console.Write(" ");
            }
            Console.WriteLine();
        }

        public static void Render(this string card)
        {
            if (card.EndsWith('D') || card.EndsWith('H'))
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.Black;

            var displayCard = card.Replace('C', '\u2663').Replace('D', '\u2666').Replace('H', '\u2665').Replace('S', '\u2660');
            Console.Write(displayCard);
        }
    }

}