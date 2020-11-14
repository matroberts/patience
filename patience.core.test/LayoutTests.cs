﻿using System;
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
                Foundation = {ClubsStack = {"AH"}}
            };

            Assert.That(() => layout.AssertInvariants(), Throws.InvalidOperationException.With.Message.EqualTo("Invariant Violation - ClubsStack contains the card 'AH' which does not match suit."));
        }

        [Test]
        public void AssertInvariants_FoundationStack_MustStartWithAnAce()
        {
            var layout = new Layout()
            {
                Foundation = { ClubsStack = { "2C" } }
            };

            Assert.That(() => layout.AssertInvariants(), Throws.InvalidOperationException.With.Message.EqualTo("Invariant Violation - ClubsStack is not in rank order, ranks are '2'."));
        }

        [Test]
        public void AssertInvariants_FoundationStack_TheStackMustBeInOrder()
        {
            var layout = new Layout()
            {
                Foundation = { ClubsStack = { "AC", "2C", "4C" } }
            };

            Assert.That(() => layout.AssertInvariants(), Throws.InvalidOperationException.With.Message.EqualTo("Invariant Violation - ClubsStack is not in rank order, ranks are '1, 2, 4'."));
        }

        [Test]
        public void AssertInvariants_FoundationStack_AllFourStacksAreChecked()
        {
            Assert.That(() => new Layout() { Foundation = { ClubsStack = { "AD" } } }.AssertInvariants(), Throws.InvalidOperationException);
            Assert.That(() => new Layout() { Foundation = { DiamondsStack = { "AH" } } }.AssertInvariants(), Throws.InvalidOperationException);
            Assert.That(() => new Layout() { Foundation = { HeartsStack = { "AS" } } }.AssertInvariants(), Throws.InvalidOperationException);
            Assert.That(() => new Layout() { Foundation = { SpadesStack = { "AC" } } }.AssertInvariants(), Throws.InvalidOperationException);
        }

        #endregion

        #region Measure and Step - together make deal and un-deal

        [Test]
        public void Measure_TheStock_ShouldGiveThePositionIfYouAdvanceThreeThreeCards()
        {
            // Arrange
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C", "6C", "7C" }, Position = 0 } // 1-indexed !!
            };

            // Act/Assert
            var (from, to) = layout.Measure();
            Assert.That(from, Is.EqualTo(0));
            Assert.That(to, Is.EqualTo(3));
        }

        [Test]
        public void Measure_WhenADealWillPassTheEndOfTheStock_TheMeasureReturnsTheEndOfTheStockAsThePosition()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C", "6C", "7C" }, Position = 6 } // deal will send you past the end of the stock
            };

            // Act/Assert
            var (from, to) = layout.Measure();
            Assert.That(from, Is.EqualTo(6));
            Assert.That(to, Is.EqualTo(7));
        }

        [Test]
        public void Measure_WhenDealHappensOnTheEndOfTheStock_TheMeasureReturnsTheBeginningOfTheStock()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C", "6C", "7C" }, Position = 7 } // last position
            };

            // Act/Assert
            var (from, to) = layout.Measure();
            Assert.That(from, Is.EqualTo(7));
            Assert.That(to, Is.EqualTo(0));
        }

        [Test]
        public void Measure_WorkCorrectly_WhenTheStockIsEmpty()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { }, Position = 0 } // last position
            };

            // Act/Assert
            var (from, to) = layout.Measure();
            Assert.That(from, Is.EqualTo(0));
            Assert.That(to, Is.EqualTo(0));
        }

        [Test]
        public void Step_ChangesTheStockPosition_ToTheToPosition()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C", "6C", "7C" }, Position = 6 }
            };

            layout.Step(6, 7);
            Assert.That(layout.Stock.Position, Is.EqualTo(7));
            Assert.That(layout.Stock.MoreStock, Is.False);

            layout.Step(7, 0);
            Assert.That(layout.Stock.Position, Is.EqualTo(0));
            Assert.That(layout.Stock.MoreStock, Is.True);

            layout.Step(0, 3);
            Assert.That(layout.Stock.Position, Is.EqualTo(3));
            Assert.That(layout.Stock.MoreStock, Is.True);
        }

        [Test]
        public void Step_IfTheFromPosition_IsNotTheCurrentPosition_AnExceptionIsThrown()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C", "6C", "7C" }, Position = 6 }
            };

            Assert.That(() => layout.Step(7, 0), Throws.ArgumentException.With.Message.EqualTo("Cannot Step from '7' since the current position is '6'"));
        }

        [Test]
        public void Step_IfTheToPosition_IsLessThanZero_AnExceptionIsThrown()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C", "6C", "7C" }, Position = 7 }
            };

            Assert.That(() => layout.Step(7, -1), Throws.ArgumentException.With.Message.EqualTo("Cannot Step to '-1' since it is before the beginning of the stock."));
        }

        [Test]
        public void Step_IfTheToPosition_IsGreaterThanTheStockLength_AnExceptionIsThrown()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C", "6C", "7C" }, Position = 6 }
            };

            Assert.That(() => layout.Step(6, 8), Throws.ArgumentException.With.Message.EqualTo("Cannot Step to '8' since it is past the end of the stock."));
        }

        #endregion

        #region Deal

        [Test]
        public void Deal_TheStock_ShouldAdvanceThreeCards()
        {
            // Arrange
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C", "6C", "7C" }, Position = 0 } // 1-indexed !!
            };

            // Act/Assert
            layout.Deal();
            Assert.That(layout.Stock.Position, Is.EqualTo(3));
            Assert.That(layout.Stock.MoreStock, Is.True);

            // Act/Assert
            layout.Deal();
            Assert.That(layout.Stock.Position, Is.EqualTo(6));
            Assert.That(layout.Stock.MoreStock, Is.True);
        }

        [Test]
        public void Deal_WhenADealPassesTheEndOfTheStock_ThePositionIsAdvancedToTheEnd()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C", "6C", "7C" }, Position = 6 } // deal will send you past the end of the stock
            };

            layout.Deal();
            Assert.That(layout.Stock.Position, Is.EqualTo(7));
            Assert.That(layout.Stock.MoreStock, Is.False);
        }

        [Test]
        public void Deal_WhenDealHappensOnTheEndOfTheStock_YouGoBackToTheBeginningOfTheStock()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C", "6C", "7C" }, Position = 7 } // last position
            };

            layout.Deal();
            Assert.That(layout.Stock.Position, Is.EqualTo(0));
            Assert.That(layout.Stock.MoreStock, Is.True);

            layout.Deal();
            Assert.That(layout.Stock.Position, Is.EqualTo(3));
            Assert.That(layout.Stock.MoreStock, Is.True);
        }

        [Test]
        public void Deal_WorkCorrectly_WhenTheStockIsEmpty()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { }, Position = 0 } // last position
            };

            layout.Deal();
            Assert.That(layout.Stock.Position, Is.EqualTo(0));
            Assert.That(layout.Stock.MoreStock, Is.False);
        }

        #endregion

        #region UnDeal - Should do the opposite of deal

        [Test]
        public void UnDeal_TheStock_ShouldRetreatThreeCards()
        {
            // Arrange
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C", "6C", "7C" }, Position = 0 } // 1-indexed !!
            };

            Assert.That(layout.Stock.MoreStock, Is.True);
            Assert.That(layout.Stock.Position, Is.EqualTo(0));

            layout.Deal();

            Assert.That(layout.Stock.MoreStock, Is.True);
            Assert.That(layout.Stock.Position, Is.EqualTo(3));

            layout.UnDeal();

            Assert.That(layout.Stock.MoreStock, Is.True);
            Assert.That(layout.Stock.Position, Is.EqualTo(0));
        }

        [Test]
        public void UnDeal_WhenUnDealHappensAtTheBeginningOfTheStock_YouGoBackToTheEndOfTheStock()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C", "6C", "7C" }, Position = 7 } // last position
            };

            Assert.That(layout.Stock.MoreStock, Is.False);
            Assert.That(layout.Stock.Position, Is.EqualTo(7));

            layout.Deal();
            Assert.That(layout.Stock.Position, Is.EqualTo(0));
            Assert.That(layout.Stock.MoreStock, Is.True);

            layout.UnDeal();
            Assert.That(layout.Stock.Position, Is.EqualTo(7));
            Assert.That(layout.Stock.MoreStock, Is.False);
        }

        // [Test]
        // public void UnDeal_WhenUnDealHappensAtTheEndOfTheStock_YouGoBackMinimumNumberOfCardsWhichMakesYouPositionAMultipleOfThree_Ouch()
        // {
        //     var layout = new Layout()
        //     {
        //         Stock = { Cards = { "AC", "2C", "3C", "4C", "5C", "6C", "7C" }, Position = 6 } // deal will send you past the end of the stock
        //     };
        //
        //     Assert.That(layout.Stock.Position, Is.EqualTo(6));
        //     Assert.That(layout.Stock.MoreStock, Is.True);
        //
        //     layout.Deal();
        //     Assert.That(layout.Stock.Position, Is.EqualTo(7));
        //     Assert.That(layout.Stock.MoreStock, Is.False);
        //
        //     layout.UnDeal();
        //     Assert.That(layout.Stock.Position, Is.EqualTo(6));
        //     Assert.That(layout.Stock.MoreStock, Is.True);
        // }

        // [Test]
        // public void UnDeal_WhenUnDealHappensAtTheEndOfTheStock_YouGoBackMinimumNumberOfCardsWhichMakesYouPositionAMultipleOfThree_Ouch()
        // {
        //     var layout = new Layout()
        //     {
        //         Stock = { Cards = { "AC", "2C", "3C", "4C", "5C", "6C", "7C", "8C" }, Position = 6 } // deal will send you past the end of the stock
        //     };
        //
        //     Assert.That(layout.Stock.Position, Is.EqualTo(6));
        //     Assert.That(layout.Stock.MoreStock, Is.True);
        //
        //     layout.Deal();
        //     Assert.That(layout.Stock.Position, Is.EqualTo(7));
        //     Assert.That(layout.Stock.MoreStock, Is.False);
        //
        //     layout.UnDeal();
        //     Assert.That(layout.Stock.Position, Is.EqualTo(6));
        //     Assert.That(layout.Stock.MoreStock, Is.True);
        // }

        // Need to also test the case when exact multple of 3
        // What about an undeal if you were at position 2? - are these positions just illegal? - infact aran't all the position

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
                    DiamondsStack = {"AD", "2D", "3D"}
                }
            };

            layout.Move("Stock", "DiamondsStack");

            Assert.That(layout.Stock.Cards, Is.Empty);
            Assert.That(layout.Stock.Position, Is.EqualTo(0));
            Assert.That(layout.Foundation.DiamondsStack, Is.EqualTo(new List<Card>{ "AD", "2D", "3D", "4D"}));
        }

        [Test]
        public void Move_ThrowsException_IfTheFromStackDoesNotExist()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "4D" }, Position = 1 },
                Foundation =
                {
                    DiamondsStack = {"AD", "2D", "3D"}
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
                    DiamondsStack = {"AD", "2D", "3D"}
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
                    DiamondsStack = {"AD", "2D", "3D"}
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
                    DiamondsStack = {"AD", "2D", "3D"}
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
                    DiamondsStack = {"AD", "2D", "3D"}
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
                    DiamondsStack = {"AD", "2D", "3D"}
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
                    DiamondsStack = {"AD", "2D", "3D"}
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
                    DiamondsStack = {"AD", "2D", "3D"}
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
                    DiamondsStack = {"AD", "2D", "3D", "4D"}
                }
            };

            // Act
            string stack = layout.IsAvailable("3D");

            // Assert
            Assert.That(stack, Is.Null);
        }

        #endregion

        #region CanFoundationAccept

        [Test]
        public void CanFoundationAccept_ReturnsTheStackName_IfTheMatchingSuitStack_HasOneRankLower()
        {
            // Arrange
            var layout = new Layout()
            {
                Foundation =
                {
                    DiamondsStack = {"AD", "2D", "3D"}
                }
            };

            // Act
            string stack = layout.CanFoundationAccept("4D");

            // Assert
            Assert.That(stack, Is.EqualTo("DiamondsStack"));
        }

        [Test]
        public void CanFoundationAccept_ReturnsTheStackName_IfTheStackIsEmpty_AndYouHaveAnAce()
        {
            // Arrange
            var layout = new Layout()
            {
                Foundation =
                {
                    DiamondsStack = {}
                }
            };

            // Act
            string stack = layout.CanFoundationAccept("AD");

            // Assert
            Assert.That(stack, Is.EqualTo("DiamondsStack"));
        }

        [Test]
        public void CanFoundationAccept_ReturnsNull_IfTheSuitDoesNotMatch()
        {
            // Arrange
            var layout = new Layout()
            {
                Foundation =
                {
                    DiamondsStack = {"AD", "2D", "3D"}
                }
            };

            // Act
            string stack = layout.CanFoundationAccept("4H");

            // Assert
            Assert.That(stack, Is.Null);
        }

        [Test]
        public void CanFoundationAccept_ReturnsNull_IfTheRankIsNotNextInLine()
        {
            // Arrange
            var layout = new Layout()
            {
                Foundation =
                {
                    DiamondsStack = {"AD", "2D", "3D"}
                }
            };

            // Act
            string stack = layout.CanFoundationAccept("5D");

            // Assert
            Assert.That(stack, Is.Null);
        }

        #endregion
    }
}