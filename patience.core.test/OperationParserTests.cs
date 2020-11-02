using System;
using System.Linq;
using NUnit.Framework;

namespace patience.core.test
{
    [TestFixture]
    public class OperationParserTests
    {
        [Test]
        public void IfOperation_IsNotUnderstood_AnErrorMessageIsReturned()
        {
            // Arrange
  

            // Act
            var parser = new OperationParser();
            var (operation, errorMessage) = parser.Parse("NotAnOperation");

            // Assert
            Assert.That(operation, Is.Null);
            Assert.That(errorMessage, Is.EqualTo("Operation 'NotAnOperation' is not understood."));
        }

        [TestCase("S")]
        [TestCase("s")]
        [TestCase("show")]
        public void An_S_IsTheShowOperation(string opString)
        {
            // Act
            var parser = new OperationParser();
            var (operation, errorMessage) = parser.Parse(opString);

            // Assert
            Assert.That(operation, Is.TypeOf<OperationShow>());
            Assert.That(errorMessage, Is.Null);
        }
        
        [TestCase("D")]
        [TestCase("d")]
        [TestCase("deal")]
        public void An_D_IsTheDealOperation(string opString)
        {
            // Act
            var parser = new OperationParser();
            var (operation, errorMessage) = parser.Parse(opString);

            // Assert
            Assert.That(operation, Is.TypeOf<OperationDeal>());
            Assert.That(errorMessage, Is.Null);
        }
    }
}