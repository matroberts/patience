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
            Assert.That(result.Layout.Stock, Is.EqualTo(new[] { "XX", "2C", "3C", "4C" }));
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
            Assert.That(result.Layout.Stock, Is.EqualTo(new []{"XX", "2C", "3C", "4C"}));
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
            Assert.That(result.Layout.Stock, Is.EqualTo(new []{"XX", "AC", "2C", "3C"}));
        }

        [Test]
        public void Print_Stock_ShouldShow2Card_IfThePositionIs2()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C" }, Position = 2 } // 1-indexed !!
            };

            var klondike = new Klondike(layout);
            var result = klondike.Operate("H");

            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result.Layout.Stock, Is.EqualTo(new[] { "XX", "AC", "2C" }));
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
            Assert.That(result.Layout.Stock, Is.EqualTo(new[] { "XX", "AC" }));
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
            Assert.That(result.Layout.Stock, Is.EqualTo(new[] { "XX" }));
        }

        [Test]
        public void Print_Stock_ShouldShowEmptyPile_IfThePositionIsAtTheEndOfTheStock()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C", "5C" }, Position = 5 } // 1-indexed !!
            };

            var klondike = new Klondike(layout);
            var result = klondike.Operate("H");

            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result.Layout.Stock, Is.EqualTo(new[] { "--", "3C", "4C", "5C" }));
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
            Assert.That(result.Layout.Stock, Is.EqualTo(new[] { "--" }));
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
                    DiamondsStack = {"AD", "2D", "3D"}
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
                    ClubsStack = {"AC", "2C", "3C"},
                    DiamondsStack = {"AD", "2D"},
                    HeartsStack = {"AH"},
                    SpadesStack = {"AS", "2S", "3S", "4S"},
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
                Stock = { Cards = { "AC", "2C", "3C", "4C" }, Position = 0 } // 1-indexed !!
            };
            var klondike = new Klondike(layout);

            // Act/Assert
            var result1 = klondike.Operate("D");
            Assert.That(result1.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result1.Layout.Stock, Is.EqualTo(new[] { "XX", "AC", "2C", "3C" }));

            // Act/Assert
            var result2 = klondike.Operate("D");
            Assert.That(result2.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result2.Layout.Stock, Is.EqualTo(new[] { "--", "2C", "3C", "4C" }));

            // Act/Assert
            var result3 = klondike.Operate("D");
            Assert.That(result3.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result3.Layout.Stock, Is.EqualTo(new[] { "XX" }));

            // Act/Assert
            var result4 = klondike.Operate("D");
            Assert.That(result4.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result4.Layout.Stock, Is.EqualTo(new[] { "XX", "AC", "2C", "3C" }));
        }

        [Test]
        public void Deal_WorkCorrectly_WhenTheStockIsEmpty()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { }, Position = 0 } // last position
            };
            var klondike = new Klondike(layout);
            var result = klondike.Operate("D");
            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result.Layout.Stock, Is.EqualTo(new[] { "--" }));
        }

        #endregion

        #region Foundation Tests

        [Test]
        public void Foundation_CanMoveAnAvailableAce_ToTheFoundation()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "AS"}, Position = 1 },
                Foundation =
                {
                    DiamondsStack = {"AD", "2D", "3D"}
                }
            };

            var klondike = new Klondike(layout);

            var result = klondike.Operate("FAS");
            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result.Layout.Stock, Is.EqualTo(new[] {"--"}));  
            Assert.That(result.Layout.Foundation, Is.EqualTo(new []{"--", "3D", "--", "AS"}));
        }

        [Test]
        public void Foundation_ReturnsAnError_IfTheMoveCannotBeMade()
        {
            var layout = new Layout()
            {
                Stock = { Cards = { "2S" }, Position = 1 },
                Foundation =
                {
                    DiamondsStack = {"AD", "2D", "3D"}
                }
            };

            var klondike = new Klondike(layout);

            var result = klondike.Operate("F2S");
            Assert.That(result.Status, Is.EqualTo(ApiStatus.Error));
            Assert.That(result.Message, Is.EqualTo("'2S' cannot be moved to the foundation."));
            Assert.That(result.Layout.Stock, Is.EqualTo(new[] { "--", "2S" }));
            Assert.That(result.Layout.Foundation, Is.EqualTo(new[] { "--", "3D", "--", "--" }));
        }

        #endregion
    }
}