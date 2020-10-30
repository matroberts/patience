using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace patience.core.test
{
    [TestFixture]
    public class CardTests
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

        [TestCase(Suit.Diamonds, 2, "2\u2662")]
        [TestCase(Suit.Hearts, 2, "2\u2661")]
        [TestCase(Suit.Spades, 2, "2\u2660")]
        [TestCase(Suit.Clubs, 1, "A\u2663")]
        [TestCase(Suit.Clubs, 2, "2\u2663")]
        [TestCase(Suit.Clubs, 3, "3\u2663")]
        [TestCase(Suit.Clubs, 4, "4\u2663")]
        [TestCase(Suit.Clubs, 5, "5\u2663")]
        [TestCase(Suit.Clubs, 6, "6\u2663")]
        [TestCase(Suit.Clubs, 7, "7\u2663")]
        [TestCase(Suit.Clubs, 8, "8\u2663")]
        [TestCase(Suit.Clubs, 9, "9\u2663")]
        [TestCase(Suit.Clubs, 10, "10\u2663")]
        [TestCase(Suit.Clubs, 11, "J\u2663")]
        [TestCase(Suit.Clubs, 12, "Q\u2663")]
        [TestCase(Suit.Clubs, 13, "K\u2663")]
        public void CardShouldDisplayCorrectly(Suit suit, int rank, string displayValue)
        {
            var card = new Card(suit, rank);
            Assert.That(card.ToString(), Is.EqualTo(displayValue));
        }
    }
}