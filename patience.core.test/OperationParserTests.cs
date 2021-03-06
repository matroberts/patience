﻿using System;
using System.Linq;
using NUnit.Framework;

namespace patience.core.test
{
    [TestFixture]
    public class OperationParserTests
    {
        #region Operation not understood

        [Test]
        public void IfOperation_IsNotUnderstood_AnErrorMessageIsReturned()
        {
            // Arrange
            var layout = new Layout();

            // Act
            var parser = new OperationParser();
            var (act, command, errorMessage) = parser.Parse(layout, "NotAnOperation");

            // Assert
            Assert.That(act, Is.EqualTo(Act.Error));
            Assert.That(command, Is.Null);
            Assert.That(errorMessage, Is.EqualTo("Operation 'NotAnOperation' is not understood."));
        }

        #endregion

        #region H - Help

        [TestCase("H")]
        [TestCase("h")]
        [TestCase("help")]
        public void An_H_IsTheHelpOperation(string opString)
        {
            // Arrange
            var layout = new Layout();

            // Act
            var parser = new OperationParser();
            var (act, command, errorMessage) = parser.Parse(layout, opString);

            // Assert
            Assert.That(act, Is.EqualTo(Act.Help));
            Assert.That(command, Is.Null);
            Assert.That(errorMessage, Is.Null);
        }

        #endregion

        #region D - Deal

        [TestCase("D")]
        [TestCase("d")]
        [TestCase("deal")]
        public void An_D_IsTheDealOperation(string opString)
        {
            // Arrange
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C" }, Position = 4 } // 1-indexed !!
            };

            // Act
            var parser = new OperationParser();
            var (act, command, errorMessage) = parser.Parse(layout, opString);

            // Assert
            Assert.That(act, Is.EqualTo(Act.Do));
            Assert.That(errorMessage, Is.Null);

            var deal = command as DealCommand;
            Assert.That(deal.From, Is.EqualTo(4));
            Assert.That(deal.To, Is.EqualTo(0));
        }

        #endregion

        #region F - Foundation

        [TestCase("FAD")]
        [TestCase("fad")]
        public void A_FoundationMove_ResultsInAMoveCommand_IfEverythingIsValid(string opString)
        {
            // Arrange
            var layout = new Layout()
            {
                Stock = { Cards = { "AD" }, Position = 1 },
                Foundation =
                {
                    DiamondsStack = {}
                }
            };

            // Act
            var parser = new OperationParser();
            var (act, command, errorMessage) = parser.Parse(layout, opString);

            // Assert
            Assert.That(act, Is.EqualTo(Act.Do));
            Assert.That(errorMessage, Is.Null);
            var move = command as MoveCommand;
            Assert.That(move, Is.Not.Null);
            Assert.That(move.From, Is.EqualTo("Stock"));
            Assert.That(move.To, Is.EqualTo("DiamondsStack"));
        }

        [Test]
        public void A_FoundationMove_ResultsInAError_IfTheCardDoesNotParse()
        {
            // Arrange
            var layout = new Layout()
            {
                Stock = { Cards = { "AD" }, Position = 1 },
                Foundation =
                {
                    DiamondsStack = {}
                }
            };

            // Act
            var parser = new OperationParser();
            var (act, command, errorMessage) = parser.Parse(layout, "FNotACard");

            // Assert
            Assert.That(act, Is.EqualTo(Act.Error));
            Assert.That(command, Is.Null);
            Assert.That(errorMessage, Is.EqualTo("'NotACard' is not recognized as a card."));
        }

        [Test]
        public void A_FoundationMove_ResultsInAError_IfTheCardIsNotAvailableToBeMoved()
        {
            // Arrange
            var layout = new Layout()
            {
                Stock = { Cards = { "AD" }, Position = 1 },
                Foundation =
                {
                    DiamondsStack = {}
                }
            };

            // Act
            var parser = new OperationParser();
            var (act, command, errorMessage) = parser.Parse(layout, "F4D");

            // Assert
            Assert.That(act, Is.EqualTo(Act.Error));
            Assert.That(command, Is.Null);
            Assert.That(errorMessage, Is.EqualTo("'4D' is not available to be moved."));
        }

        [Test]
        public void A_FoundationMove_ResultsInAError_IfTheFoundationCannotAcceptTheCard()
        {
            // Arrange
            var layout = new Layout()
            {
                Stock = { Cards = { "4D" }, Position = 1 },
                Foundation =
                {
                    DiamondsStack = {}
                }
            };

            // Act
            var parser = new OperationParser();
            var (act, command, errorMessage) = parser.Parse(layout, "F4D");

            // Assert
            Assert.That(act, Is.EqualTo(Act.Error));
            Assert.That(command, Is.Null);
            Assert.That(errorMessage, Is.EqualTo($"'4D' cannot be moved to the foundation."));
        }

        [Test]
        public void A_FoundationMove_ResultsInAError_IfTheCardIsAlreadyTopOfTheFoundation()
        {
            // Arrange
            var layout = new Layout()
            {
                Foundation =
                {
                    DiamondsStack = { "AD" }
                }
            };

            // Act
            var parser = new OperationParser();
            var (act, command, errorMessage) = parser.Parse(layout, "FAD");

            // Assert
            Assert.That(act, Is.EqualTo(Act.Error));
            Assert.That(command, Is.Null);
            Assert.That(errorMessage, Is.EqualTo($"'AD' cannot be moved to the foundation."));
        }

        #endregion

        #region <card> - If you just specify the card, it will attempt to move it

        [TestCase("AD")]
        [TestCase("ad")]
        public void A_CardMove_ResultsInAMoveCommand_FromStockToFoundation_IfEverythingIsValid(string opString)
        {
            // Arrange
            var layout = new Layout()
            {
                Stock = { Cards = { "AD" }, Position = 1 },
                Foundation =
                {
                    DiamondsStack = {}
                }
            };

            // Act
            var parser = new OperationParser();
            var (act, command, errorMessage) = parser.Parse(layout, opString);

            // Assert
            Assert.That(act, Is.EqualTo(Act.Do));
            Assert.That(errorMessage, Is.Null);
            var move = command as MoveCommand;
            Assert.That(move, Is.Not.Null);
            Assert.That(move.From, Is.EqualTo("Stock"));
            Assert.That(move.To, Is.EqualTo("DiamondsStack"));
        }

        [TestCase("6H")]
        [TestCase("6h")]
        public void A_CardMove_ResultsInAMoveCommand_FromTableauToTableau_IfEverythingIsValid(string opString)
        {
            // Arrange
            var layout = new Layout()
            {
                Tableau =
                {
                    T1Stack = {Cards = {"7C"}, FlippedAt = 1},
                    T2Stack = {Cards = {"7S", "6H"}, FlippedAt = 1}
                }
            };

            // Act
            var parser = new OperationParser();
            var (act, command, errorMessage) = parser.Parse(layout, opString);

            // Assert
            Assert.That(act, Is.EqualTo(Act.Do), () => errorMessage);
            Assert.That(errorMessage, Is.Null);
            var move = command as MoveCommand;
            Assert.That(move, Is.Not.Null);
            Assert.That(move.From, Is.EqualTo("T2Stack"));
            Assert.That(move.To, Is.EqualTo("T1Stack"));
        }

        [Test]
        public void A_Move_ResultsInAError_IfTheCardIsNotAvailableToBeMoved()
        {
            // Arrange
            var layout = new Layout()
            {
                Stock = { Cards = { "AD" }, Position = 1 },
                Foundation =
                {
                    DiamondsStack = {}
                }
            };

            // Act
            var parser = new OperationParser();
            var (act, command, errorMessage) = parser.Parse(layout, "4D");

            // Assert
            Assert.That(act, Is.EqualTo(Act.Error));
            Assert.That(command, Is.Null);
            Assert.That(errorMessage, Is.EqualTo("'4D' is not available to be moved."));
        }

        [Test]
        public void A_Move_ResultsInAError_IfNoWhereCanAcceptTheCard()
        {
            // Arrange
            var layout = new Layout()
            {
                Stock = { Cards = { "4D" }, Position = 1 },
                Foundation =
                {
                    DiamondsStack = {}
                }
            };

            // Act
            var parser = new OperationParser();
            var (act, command, errorMessage) = parser.Parse(layout, "4D");

            // Assert
            Assert.That(act, Is.EqualTo(Act.Error));
            Assert.That(command, Is.Null);
            Assert.That(errorMessage, Is.EqualTo($"'4D' cannot be moved anywhere."));
        }

        #endregion

        #region U - Undo

        [TestCase("U")]
        [TestCase("u")]
        [TestCase("undo")]
        public void An_U_IsTheUndoOperation(string opString)
        {
            // Arrange
            var layout = new Layout()
            {
                Stock = { Cards = { "AC", "2C", "3C", "4C" }, Position = 4 } // 1-indexed !!
            };

            // Act
            var parser = new OperationParser();
            var (act, command, errorMessage) = parser.Parse(layout, opString);

            // Assert
            Assert.That(act, Is.EqualTo(Act.Undo));
            Assert.That(command, Is.Null);
            Assert.That(errorMessage, Is.Null);
        }

        #endregion
    }
}