using System;
using System.Linq;
using NUnit.Framework;

namespace patience.core.test
{
    [TestFixture]
    public class DisplayTest
    {
        [Test]
        public void TheUnicodeCharacters_ForTheCardSuits_ShouldDisplayInResharperTestRunner()
        {
            char club = '\u2663';
            char diamond = '\u2662';
            char heart = '\u2661';
            char spade = '\u2660';
            Console.WriteLine(club);
            Console.WriteLine(diamond);
            Console.WriteLine(heart);
            Console.WriteLine(spade);
            // Console colors do not work in resharper test runner
            // Though presumably they would in a real console
            // Console.BackgroundColor = ConsoleColor.Blue;
            // Console.ForegroundColor = ConsoleColor.White;
        }
    }
}