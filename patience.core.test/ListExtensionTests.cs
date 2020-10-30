using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace patience.core.test
{
    [TestFixture]
    public class ListExtensionTests
    {
        [Test]
        public void Shuffle_WorksCorrectlyWhenOneElement()
        {
            var list = new int[] {1};
            list.Shuffle();

            Assert.That(list, Is.EqualTo(new int[]{1}));
        }

        [Test]
        public void Shuffle_AfterShuffling_TheArrayContainsTheSameElements()
        {
            var list = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            list.Shuffle();

            Assert.That(list, Is.EquivalentTo(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }));
            Assert.That(list, Is.Not.EqualTo(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }));
        }
    }
}