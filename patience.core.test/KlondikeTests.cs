﻿using System;
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
        public void IfOperation_IsNotUnderstood_AnErrorMessageIsReturned_AlongWithAnUnalertedLayout()
        {
            // Arrange
            var layout = new Layout()
            {
                Stock = new Stock() { Cards = { "AC", "2C", "3C", "4C", "5C" }, Position = 4 } 
            };

            // Act
            var klondike = new Klondike(layout);
            var result = klondike.Operate("NotAnOperation");

            // Assert
            Assert.That(result.Status, Is.EqualTo(ApiStatus.OperationNotUnderstood));
            Assert.That(result.ErrorMessage, Is.EqualTo("Operation 'NotAnOperation' is not understood.  Allowed operations are P,D,U,R,<card>T, <card>TT, <card>F."));
            Assert.That(result.Layout.Stock, Is.EqualTo(new[] { "2C", "3C", "4C" }));
        }

        #endregion

        #region PrintTests


        [Test]
        public void Print_Stock_ShouldShow3Card_FromThePositionAndThePreviousTwoCards()
        {
            var layout = new Layout()
            {
                Stock = new Stock() { Cards = {"AC", "2C", "3C", "4C", "5C"}, Position = 4 } // 1-indexed !!
            };

            var klondike = new Klondike(layout);
            var result = klondike.Operate("P");

            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result.Layout.Stock, Is.EqualTo(new []{"2C", "3C", "4C"}));
        }        
        
        [Test]
        public void Print_Stock_ShouldShow3Card_IfThePositionIs3()
        {
            var layout = new Layout()
            {
                Stock = new Stock() { Cards = {"AC", "2C", "3C", "4C", "5C"}, Position = 3 } // 1-indexed !!
            };

            var klondike = new Klondike(layout);
            var result = klondike.Operate("P");

            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result.Layout.Stock, Is.EqualTo(new []{"AC", "2C", "3C"}));
        }

        [Test]
        public void Print_Stock_ShouldShow2Card_IfThePositionis2()
        {
            var layout = new Layout()
            {
                Stock = new Stock() { Cards = { "AC", "2C", "3C", "4C", "5C" }, Position = 2 } // 1-indexed !!
            };

            var klondike = new Klondike(layout);
            var result = klondike.Operate("P");

            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result.Layout.Stock, Is.EqualTo(new[] { "AC", "2C" }));
        }

        [Test]
        public void Print_Stock_ShouldShow1Card_IfThePositionIs1()
        {
            var layout = new Layout()
            {
                Stock = new Stock() { Cards = { "AC", "2C", "3C", "4C", "5C" }, Position = 1 } // 1-indexed !!
            };

            var klondike = new Klondike(layout);
            var result = klondike.Operate("P");

            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result.Layout.Stock, Is.EqualTo(new[] { "AC" }));
        }

        [Test]
        public void Print_Stock_ShouldShow0Card_IfThePositionIs0()
        {
            var layout = new Layout()
            {
                Stock = new Stock() { Cards = { "AC", "2C", "3C", "4C", "5C" }, Position = 0 } // 1-indexed !!
            };

            var klondike = new Klondike(layout);
            var result = klondike.Operate("P");

            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result.Layout.Stock, Is.EqualTo(new List<string>()));
        }

        [Test]
        public void Print_Stock_ShouldShow0Card_IfTheStockIsEmpty()
        {
            var layout = new Layout()
            {
                Stock = new Stock() { Cards = {  }, Position = 0 } // 1-indexed !!
            };

            var klondike = new Klondike(layout);
            var result = klondike.Operate("P");

            Assert.That(result.Status, Is.EqualTo(ApiStatus.Ok));
            Assert.That(result.Layout.Stock, Is.EqualTo(new List<string>()));
        }


        #endregion
    }
}