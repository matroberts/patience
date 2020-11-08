using System;
using System.Linq;
using NUnit.Framework;

namespace patience.core.test
{
    [TestFixture]
    public class OperationParserTests
    {
        #region old

        [Test]
        public void IfOperation_IsNotUnderstood_AnErrorMessageIsReturned_old()
        {
            // Arrange
  

            // Act
            var parser = new OperationParser();
            var (operation, errorMessage) = parser.Parse("NotAnOperation");

            // Assert
            Assert.That(operation, Is.Null);
            Assert.That(errorMessage, Is.EqualTo("Operation 'NotAnOperation' is not understood."));
        }

        [TestCase("H")]
        [TestCase("h")]
        [TestCase("help")]
        public void An_H_IsTheHelpOperation_old(string opString)
        {
            // Act
            var parser = new OperationParser();
            var (operation, errorMessage) = parser.Parse(opString);

            // Assert
            Assert.That(operation, Is.TypeOf<OperationHelp>());
            Assert.That(errorMessage, Is.Null);
        }
        
        [TestCase("D")]
        [TestCase("d")]
        [TestCase("deal")]
        public void An_D_IsTheDealOperation_old(string opString)
        {
            // Act
            var parser = new OperationParser();
            var (operation, errorMessage) = parser.Parse(opString);

            // Assert
            Assert.That(operation, Is.TypeOf<OperationDeal>());
            Assert.That(errorMessage, Is.Null);
        }

        #endregion

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

        [TestCase("D")]
        [TestCase("d")]
        [TestCase("deal")]
        public void An_D_IsTheDealOperation(string opString)
        {
            // Arrange
            var layout = new Layout();

            // Act
            var parser = new OperationParser();
            var (act, command, errorMessage) = parser.Parse(layout, opString);

            // Assert
            Assert.That(act, Is.EqualTo(Act.Do));
            Assert.That(command, Is.TypeOf<DealCommand>());
            Assert.That(errorMessage, Is.Null);
        }
    }
}