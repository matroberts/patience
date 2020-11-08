using System;
using System.Collections.Generic;
using System.Threading.Channels;
using NUnit.Framework;

namespace patience.core.test
{
    [TestFixture]
    public class KlondikeTests
    {
        #region GeneralTests

        [Test]
        public void AssertInvariant_IsCalledOnConstruction_ToCheckTheLayoutIsSetupProperly()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C" }, Position = -1 }
            };

            Assert.That(() => new Klondike(layout), Throws.InvalidOperationException.With.Message.EqualTo("Invariant Violation - Stock Position -1 is less than 0."));
        }

        [Test]
        public void IfOperation_IsNotUnderstood_AnErrorMessageIsReturned_AlongWithAnUnchangedLayout()
        {
            // Arrange
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C" }, Position = 4 } 
            };

            // Act
            var klondike = new Klondike(layout);
            var result = klondike.Operate("NotAnOperation");

            // Assert
            Assert.That(result.Status, Is.EqualTo(ApiStatus.Error));
            Assert.That(result.Message, Is.EqualTo("Operation 'NotAnOperation' is not understood."));
            Assert.That(result.Layout.Stock, Is.EqualTo(new[] { "2C", "3C", "4C" }));
        }

        #endregion

        #region Help Tests - Help also just returns the state of the layout, so can use it for testing layout mapping

        [Test]
        public void Help_ReturnsHelpInfo_InTheMessage()
        {
            var layout = new Layout();

            var klondike = new Klondike(layout);
            var result = klondike.Operate("H");

            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result.Layout, Is.Not.Null);
            Assert.That(result.Message, Is.EqualTo(@"
Ctrl+C Exit
H      Help
D      Deal - turn over 3 cards from the stock"));
        }

        [Test]
        public void Print_Stock_ShouldShow3Card_FromThePositionAndThePreviousTwoCards()
        {
            var layout = new Layout()
            {
                Stock = { Cards = {"AC", "2C", "3C", "4C", "5C"}, Position = 4 } // 1-indexed !!
            };

            var klondike = new Klondike(layout);
            var result = klondike.Operate("H");

            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result.Layout.Stock, Is.EqualTo(new []{"2C", "3C", "4C"}));
        }        
        
        [Test]
        public void Print_Stock_ShouldShow3Card_IfThePositionIs3()
        {
            var layout = new Layout()
            {
                Stock = { Cards = {"AC", "2C", "3C", "4C", "5C"}, Position = 3 } // 1-indexed !!
            };

            var klondike = new Klondike(layout);
            var result = klondike.Operate("H");

            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result.Layout.Stock, Is.EqualTo(new []{"AC", "2C", "3C"}));
        }

        [Test]
        public void Print_Stock_ShouldShow2Card_IfThePositionis2()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C" }, Position = 2 } // 1-indexed !!
            };

            var klondike = new Klondike(layout);
            var result = klondike.Operate("H");

            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result.Layout.Stock, Is.EqualTo(new[] { "AC", "2C" }));
        }

        [Test]
        public void Print_Stock_ShouldShow1Card_IfThePositionIs1()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C" }, Position = 1 } // 1-indexed !!
            };

            var klondike = new Klondike(layout);
            var result = klondike.Operate("H");

            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result.Layout.Stock, Is.EqualTo(new[] { "AC" }));
        }

        [Test]
        public void Print_Stock_ShouldShow0Card_IfThePositionIs0()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C" }, Position = 0 } // 1-indexed !!
            };

            var klondike = new Klondike(layout);
            var result = klondike.Operate("H");

            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result.Layout.Stock, Is.Empty);
        }

        [Test]
        public void Print_Stock_ShouldShow0Card_IfTheStockIsEmpty()
        {
            var layout = new Layout()
            {
                Stock = { Cards = {  }, Position = 0 } // 1-indexed !!
            };

            var klondike = new Klondike(layout);
            var result = klondike.Operate("H");

            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result.Layout.Stock, Is.Empty);
        }

        [Test]
        public void Print_Foundation_WhenAFoundationStackIsEmpty_DashesAreReturned()
        {
            var layout = new Layout()
            {
                Foundation = { }
            };

            var klondike = new Klondike(layout);
            var result = klondike.Operate("H");

            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result.Layout.Foundation, Is.EqualTo(new []{"--", "--", "--", "--"}));
        }

        [Test]
        public void Print_Foundation_ShowsTheTopCardInAEachFoundationStack()
        {
            var layout = new Layout()
            {
                Foundation =
                {
                    DiamondStack = {"AD", "2D", "3D"}
                }
            };

            var klondike = new Klondike(layout);
            var result = klondike.Operate("H");

            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result.Layout.Foundation, Is.EqualTo(new []{"--", "3D", "--", "--"}));
        }

        [Test]
        public void Print_Foundation_ShouldShowAllTheFoundationStacks()
        {
            var layout = new Layout()
            {
                Foundation =
                {
                    ClubStack = {"AC", "2C", "3C"},
                    DiamondStack = {"AD", "2D"},
                    HeartStack = {"AH"},
                    SpadeStack = {"AS", "2S", "3S", "4S"},
                }
            };

            var klondike = new Klondike(layout);
            var result = klondike.Operate("H");

            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result.Layout.Foundation, Is.EqualTo(new[] { "3C", "2D", "AH", "4S" }));
        }

        #endregion

        #region DealTests

        [Test]
        public void Deal_TheStock_ShouldAdvanceThreeCards()
        {
            // Arrange
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C", "6C", "7C" }, Position = 0 } // 1-indexed !!
            };
            var klondike = new Klondike(layout);

            // Act/Assert
            var result1 = klondike.Operate("D");
            Assert.That(result1.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result1.Layout.Stock, Is.EqualTo(new[] { "AC", "2C", "3C" }));
            Assert.That(result1.Layout.MoreStock, Is.True);

            // Act/Assert
            var result2 = klondike.Operate("D");
            Assert.That(result2.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result2.Layout.Stock, Is.EqualTo(new[] { "4C", "5C", "6C" }));
            Assert.That(result1.Layout.MoreStock, Is.True);
        }

        [Test]
        public void Deal_WhenADealPassesTheEndOfTheStock_TheEndOfTheStockIsShown()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C", "6C", "7C" }, Position = 6 } // deal will send you past the end of the stock
            };

            var klondike = new Klondike(layout);
            var result = klondike.Operate("D");

            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result.Layout.Stock, Is.EqualTo(new[] { "5C", "6C", "7C" }));
            Assert.That(result.Layout.MoreStock, Is.False);
        }

        [Test]
        public void Deal_WhenDealHappensOnTheEndOfTheStock_YouGoBackToTheBeginningOfTheStock()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C", "6C", "7C" }, Position = 7 } // last position
            };

            var klondike = new Klondike(layout);

            // Act/Assert
            var result1 = klondike.Operate("D");
            Assert.That(result1.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result1.Layout.Stock, Is.Empty);  // Note that you are back at position=0 here, and no cards are shown....effectively this the stock being reset
            Assert.That(result1.Layout.MoreStock, Is.True);

            // Act/Assert
            var result2 = klondike.Operate("D");
            Assert.That(result2.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result2.Layout.Stock, Is.EqualTo(new[] { "AC", "2C", "3C" }));
            Assert.That(result1.Layout.MoreStock, Is.True);
        }

        [Test]
        public void Deal_WorkCorrectly_WhenTheStockIsEmpty()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { }, Position = 0 } // last position
            };

            var klondike = new Klondike(layout);

            var result1 = klondike.Operate("D");
            Assert.That(result1.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result1.Layout.Stock, Is.Empty);  // Note that you are back at position=0 here, and no cards are shown....effectively this the stock being reset
            Assert.That(result1.Layout.MoreStock, Is.False);
        }

        #endregion
    }
}