using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business.Tests.MoneyTests
{
    [TestFixture]
    class RateTests
    {
        [Test]
        public void RateConstructorOK()
        {
            //Arrange
            Rate r = new Rate(Currency.GBP, Currency.EUR, 0.7m);

            //Assert
            Assert.NotNull(r);
        }

        [Test]
        public void RateConstructorFail()
        {
            //Assert
            var ex = Assert.Throws<ArgumentException>(() => new Rate(Currency.AUD, Currency.CAD, 0));
            Assert.AreEqual(ExceptionMessages.RateNotLessZero(), ex.Message);
        }
    }
}
