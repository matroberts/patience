using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace patience.core.test
{
    [TestFixture]
    public class LayoutTests
    {
        #region AssertInvariants

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

            Assert.That(() => layout.AssertInvariants(), Throws.InvalidOperationException.With.Message.EqualTo("Invariant Violation - ClubsStack contains the card 'AH' which does not match suit."));
        }

        [Test]
        public void AssertInvariants_FoundationStack_MustStartWithAnAce()
        {
            var layout = new Layout()
            {
                Foundation = { ClubStack = { "2C" } }
            };

            Assert.That(() => layout.AssertInvariants(), Throws.InvalidOperationException.With.Message.EqualTo("Invariant Violation - ClubsStack is not in rank order, ranks are '2'."));
        }

        [Test]
        public void AssertInvariants_FoundationStack_TheStackMustBeInOrder()
        {
            var layout = new Layout()
            {
                Foundation = { ClubStack = { "AC", "2C", "4C" } }
            };

            Assert.That(() => layout.AssertInvariants(), Throws.InvalidOperationException.With.Message.EqualTo("Invariant Violation - ClubsStack is not in rank order, ranks are '1, 2, 4'."));
        }

        [Test]
        public void AssertInvariants_FoundationStack_AllFourStacksAreChecked()
        {
            Assert.That(() => new Layout() { Foundation = { ClubStack = { "AD" } } }.AssertInvariants(), Throws.InvalidOperationException);
            Assert.That(() => new Layout() { Foundation = { DiamondStack = { "AH" } } }.AssertInvariants(), Throws.InvalidOperationException);
            Assert.That(() => new Layout() { Foundation = { HeartStack = { "AS" } } }.AssertInvariants(), Throws.InvalidOperationException);
            Assert.That(() => new Layout() { Foundation = { SpadeStack = { "AC" } } }.AssertInvariants(), Throws.InvalidOperationException);
        }

        #endregion

        #region Move

        [Test]
        public void Move_FromStockToFoundation_ShouldWork_IfInvariantsNotViolated()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "4D"}, Position = 1 },
                Foundation =
                {
                    DiamondStack = {"AD", "2D", "3D"}
                }
            };

            layout.Move("Stock", "DiamondsStack");

            Assert.That(layout.Stock.Cards, Is.Empty);
            Assert.That(layout.Stock.Position, Is.EqualTo(0));
            Assert.That(layout.Foundation.DiamondStack, Is.EqualTo(new List<Card>{ "AD", "2D", "3D", "4D"}));
        }

        [Test]
        public void Move_ThrowsException_IfTheFromStackDoesNotExist()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "4D" }, Position = 1 },
                Foundation =
                {
                    DiamondStack = {"AD", "2D", "3D"}
                }
            };

            Assert.That(() => layout.Move("NotExists", "DiamondsStack"), Throws.ArgumentException.With.Message.EqualTo("From stack 'NotExists' does not exist."));
        }

        [Test]
        public void Move_ThrowsException_IfTheToStackDoesNotExist()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "4D" }, Position = 1 },
                Foundation =
                {
                    DiamondStack = {"AD", "2D", "3D"}
                }
            };

            Assert.That(() => layout.Move("Stock", "NotExists"), Throws.ArgumentException.With.Message.EqualTo("To stack 'NotExists' does not exist."));
        }

        [Test]
        public void Move_FromStock_ThrowsException_IfThereIsNoCardToTakeFromTheStock()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "4D" }, Position = 0 },
                Foundation =
                {
                    DiamondStack = {"AD", "2D", "3D"}
                }
            };

            Assert.That(() => layout.Move("Stock", "DiamondsStack"), Throws.ArgumentException.With.Message.EqualTo("The stock has no card to take.")); ;
        }

        [Test]
        public void Move_ToFoundation_ThrowsException_IfThereIsASuitMismatch()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "4C" }, Position = 1 },
                Foundation =
                {
                    DiamondStack = {"AD", "2D", "3D"}
                }
            };

            Assert.That(() => layout.Move("Stock", "DiamondsStack"), Throws.ArgumentException.With.Message.EqualTo("Cannot give card '4C' to the DiamondsStack because the suit is wrong.")); ;
        }

        [Test]
        public void Move_ToFoundation_ThrowsException_IfTheRankIsWrong()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "5D" }, Position = 1 },
                Foundation =
                {
                    DiamondStack = {"AD", "2D", "3D"}
                }
            };

            Assert.That(() => layout.Move("Stock", "DiamondsStack"), Throws.ArgumentException.With.Message.EqualTo("Cannot give card '5D' to the DiamondsStack because the rank is wrong.")); ;
        }

        #endregion

        #region IsAvailable

        [Test]
        public void IsAvailable_FromStock_ReturnsTheStackName_IfThePositionIsAtTheCard()
        {
            // Arrange
            var layout = new Layout()
            {
                Stock = { Cards = { "4D" }, Position = 1 },
                Foundation =
                {
                    DiamondStack = {"AD", "2D", "3D"}
                }
            };

            // Act
            string stack = layout.IsAvailable("4D");

            // Assert
            Assert.That(stack, Is.EqualTo("Stock"));
        }

        [Test]
        public void IsAvailable_FromStock_ReturnsNull_IfTheCardIsNotAtPosition()
        {
            // Arrange
            var layout = new Layout()
            {
                Stock = { Cards = { "4D" }, Position = 0 },
                Foundation =
                {
                    DiamondStack = {"AD", "2D", "3D"}
                }
            };

            // Act
            string stack = layout.IsAvailable("4D");

            // Assert
            Assert.That(stack, Is.Null);
        }

        [Test]
        public void IsAvailable_FromFoundation_ReturnsTheStackName_IfTheCardIsOnTopOfStack()
        {
            // Arrange
            var layout = new Layout()
            {
                Foundation =
                {
                    DiamondStack = {"AD", "2D", "3D"}
                }
            };

            // Act
            string stack = layout.IsAvailable("3D");

            // Assert
            Assert.That(stack, Is.EqualTo("DiamondsStack"));
        }

        [Test]
        public void IsAvailable_FromFoundation_ReturnsNull_IfTheCardIsNotOnTop()
        {
            // Arrange
            var layout = new Layout()
            {
                Foundation =
                {
                    DiamondStack = {"AD", "2D", "3D", "4D"}
                }
            };

            // Act
            string stack = layout.IsAvailable("3D");

            // Assert
            Assert.That(stack, Is.Null);
        }

        #endregion
    }
}