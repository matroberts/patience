using System;
using System.Linq;
using NUnit.Framework;

namespace patience.core.test
{
    [TestFixture]
    public class LayoutTests
    {
        [Test]
        public void AssertInvariants_IfStockPositionIsLessThanZero_AnExceptionIsThrown()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C" }, Position = -1 }
            };

            Assert.That(() => layout.AssertInvariants(), Throws.InvalidOperationException.With.Message.EqualTo("Invariant Violation - Stock Position -1 is less than 0."));
        }

        [Test]
        public void AssertInvariants_IfStockPositionIsGreaterThanStockLength_AnExceptionIsThrown()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C" }, Position = 6 }
            };

            Assert.That(() => layout.AssertInvariants(), Throws.InvalidOperationException.With.Message.EqualTo("Invariant Violation - Stock Position 6 is greater than the stock count 5."));
        }

        [Test]
        public void AssertInvariants_FoundationStack_CardSuitsMustMatchTheStackSuit()
        {
            var layout = new Layout()
            {
                Foundation = {ClubStack = {"AH"}}
            };

            Assert.That(() => layout.AssertInvariants(), Throws.InvalidOperationException.With.Message.EqualTo("Invariant Violation - FoundationStack Clubs contains the card 'AH' which does not match suit."));
        }

        [Test]
        public void AssertInvariants_FoundationStack_MustStartWithAnAce()
        {
            var layout = new Layout()
            {
                Foundation = { ClubStack = { "2C" } }
            };

            Assert.That(() => layout.AssertInvariants(), Throws.InvalidOperationException.With.Message.EqualTo("Invariant Violation - FoundationStack Clubs is not in rank order, ranks are '2'."));
        }

        [Test]
        public void AssertInvariants_FoundationStack_TheStackMustBeInOrder()
        {
            var layout = new Layout()
            {
                Foundation = { ClubStack = { "AC", "2C", "4C" } }
            };

            Assert.That(() => layout.AssertInvariants(), Throws.InvalidOperationException.With.Message.EqualTo("Invariant Violation - FoundationStack Clubs is not in rank order, ranks are '1, 2, 4'."));
        }

        [Test]
        public void AssertInvariants_FoundationStack_AllFourStacksAreChecked()
        {
            Assert.That(() => new Layout() { Foundation = { ClubStack = { "AD" } } }.AssertInvariants(), Throws.InvalidOperationException);
            Assert.That(() => new Layout() { Foundation = { DiamondStack = { "AH" } } }.AssertInvariants(), Throws.InvalidOperationException);
            Assert.That(() => new Layout() { Foundation = { HeartStack = { "AS" } } }.AssertInvariants(), Throws.InvalidOperationException);
            Assert.That(() => new Layout() { Foundation = { SpadeStack = { "AC" } } }.AssertInvariants(), Throws.InvalidOperationException);
        }
    }
}