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
            string back = char.ConvertFromUtf32(0x1F0A0);
            Console.WriteLine(club);
            Console.WriteLine(diamond);
            Console.WriteLine(heart);
            Console.WriteLine(spade);
            Console.WriteLine(back);
            // Console colors do not work in resharper test runner
            // Though presumably they would in a real console
            // Console.BackgroundColor = ConsoleColor.Blue;
            // Console.ForegroundColor = ConsoleColor.White;
        }

        #region ToString

        [TestCase(Suit.Diamonds, 2, "2D")]
        [TestCase(Suit.Hearts, 2, "2H")]
        [TestCase(Suit.Spades, 2, "2S")]
        [TestCase(Suit.Clubs, 1, "AC")]
        [TestCase(Suit.Clubs, 2, "2C")]
        [TestCase(Suit.Clubs, 3, "3C")]
        [TestCase(Suit.Clubs, 4, "4C")]
        [TestCase(Suit.Clubs, 5, "5C")]
        [TestCase(Suit.Clubs, 6, "6C")]
        [TestCase(Suit.Clubs, 7, "7C")]
        [TestCase(Suit.Clubs, 8, "8C")]
        [TestCase(Suit.Clubs, 9, "9C")]
        [TestCase(Suit.Clubs, 10, "10C")]
        [TestCase(Suit.Clubs, 11, "JC")]
        [TestCase(Suit.Clubs, 12, "QC")]
        [TestCase(Suit.Clubs, 13, "KC")]
        public void CardShouldDisplayCorrectly(Suit suit, int rank, string displayValue)
        {
            var card = new Card(suit, rank);
            Assert.That(card.ToString(), Is.EqualTo(displayValue));
        }

        #endregion

        #region implicit cast from string

        [Test]
        public void ImplicitCast_WillThrow_IfTheStringTooShort()
        {
            Assert.That(() => { Card card = "A"; }, Throws.ArgumentException.With.Message.EqualTo("String to short in implicit cast to Card"));
        }

        [Test]
        public void ImplicitCast_WillThrow_IfTheStringTooLong()
        {
            Assert.That(() => { Card card = "999C"; }, Throws.ArgumentException.With.Message.EqualTo("String to long in implicit cast to Card"));
        }

        [TestCase("2C", Suit.Clubs)]
        [TestCase("2D", Suit.Diamonds)]
        [TestCase("2H", Suit.Hearts)]
        [TestCase("2S", Suit.Spades)]
        public void ImplicitCast_CorrectlyIdentifies_TheSuit(string str, Suit suit)
        {
            Card card = str;
            Assert.That(card.Suit, Is.EqualTo(suit));
        }

        [Test]
        public void ImplicitCast_WillThrow_IfTheSuitCharacterIsNotAllowed()
        {
            Assert.That(() => { Card card = "2c"; }, Throws.ArgumentException.With.Message.EqualTo("Suit 'c' is not recognized.  Allowed values are C,D,H,S."));
        }

        [TestCase("AC", 1)]
        [TestCase("2C", 2)]
        [TestCase("3C", 3)]
        [TestCase("4C", 4)]
        [TestCase("5C", 5)]
        [TestCase("6C", 6)]
        [TestCase("7C", 7)]
        [TestCase("8C", 8)]
        [TestCase("9C", 9)]
        [TestCase("10C", 10)]
        [TestCase("JC", 11)]
        [TestCase("QC", 12)]
        [TestCase("KC", 13)]
        public void ImplicitCast_CorrectlyIdentifies_TheFourRank(string str, int rank)
        {
            Card card = str;
            Assert.That(card.Rank, Is.EqualTo(rank));
        }

        [Test]
        public void ImplicitCast_WillThrow_IfTheRankStringIsNotAllowed()
        {
            Assert.That(() => { Card card = "11C"; }, Throws.ArgumentException.With.Message.EqualTo("Rank '11' is not recognized.  Allowed values are A,2,3,4,5,6,7,8,9,10,J,Q,K."));
        }

        [Test]
        public void Card_HasAnImplicitConversionFromString()
        {
            var cards = new List<Card> {"2C", "KD"};
            foreach (var card in cards)
            {
                Console.WriteLine(card);
            }
        }

        #endregion
    }
}