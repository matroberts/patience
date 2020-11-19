using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace patience.core.test
{
    [TestFixture]
    public class LayoutFactoryTests
    {
        [Test]
        public void CreateDeck_ShouldMakeADeckOf52Cards_WithFourSuits_AndThirteenRanks()
        {
            var cards = new LayoutFactory().CreateDeck();

            Assert.That(cards.Count(), Is.EqualTo(52));
            Assert.That(cards.Count(c => c.Suit == Suit.Clubs), Is.EqualTo(13));
            Assert.That(cards.Count(c => c.Suit == Suit.Diamonds), Is.EqualTo(13));
            Assert.That(cards.Count(c => c.Suit == Suit.Hearts), Is.EqualTo(13));
            Assert.That(cards.Count(c => c.Suit == Suit.Spades), Is.EqualTo(13));
            Assert.That(cards.Count(c => c.Rank == 1), Is.EqualTo(4));
            Assert.That(cards.Count(c => c.Rank == 2), Is.EqualTo(4));
            Assert.That(cards.Count(c => c.Rank == 3), Is.EqualTo(4));
            Assert.That(cards.Count(c => c.Rank == 4), Is.EqualTo(4));
            Assert.That(cards.Count(c => c.Rank == 5), Is.EqualTo(4));
            Assert.That(cards.Count(c => c.Rank == 6), Is.EqualTo(4));
            Assert.That(cards.Count(c => c.Rank == 7), Is.EqualTo(4));
            Assert.That(cards.Count(c => c.Rank == 8), Is.EqualTo(4));
            Assert.That(cards.Count(c => c.Rank == 9), Is.EqualTo(4));
            Assert.That(cards.Count(c => c.Rank == 10), Is.EqualTo(4));
            Assert.That(cards.Count(c => c.Rank == 11), Is.EqualTo(4));
            Assert.That(cards.Count(c => c.Rank == 12), Is.EqualTo(4));
            Assert.That(cards.Count(c => c.Rank == 13), Is.EqualTo(4));
        }

        [Test]
        public void ShuffleTheDeck()
        {
            var cards = new LayoutFactory().CreateDeck();
            cards.Shuffle();

            foreach (var card in cards)
            {
                Console.WriteLine(card);
            }
        }

        [Test]
        public void Create_ShouldMakeALayoutWith52Cards_AndEachCardIsUnique()
        {
            var layout = new LayoutFactory().Create();

            var cards = layout.Stacks.SelectMany(s => s.Cards);

            Assert.That(cards.Count(), Is.EqualTo(52));
            Assert.That(cards.Distinct().Count(), Is.EqualTo(cards.Count()));
        }
    }
}