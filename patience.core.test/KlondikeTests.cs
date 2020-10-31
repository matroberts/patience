﻿using NUnit.Framework;

namespace patience.core.test
{
    [TestFixture]
    public class KlondikeTests
    {
        [Test]
        public void METHOD()
        {
            var layout = new Layout()
            {
                Stock = new Stock() { Cards = {"AC", "2C", "3C", "4C", "5C"} }
            };

            var klondike = new Klondike(layout);
            var result = klondike.Operate("P");

            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
           // Assert.That(result.Layout, Is.EqualTo());

        }
    }
}