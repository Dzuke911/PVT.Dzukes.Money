using NUnit.Framework;
using PVT.Money.Business.MoneyClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business.Tests.MoneyTests
{
    [TestFixture]
    public class OldGlobalRatesTests
    {
        [Test]
        public void GlobalRatesConstructorOK()
        {
            IOldGlobalRates gRates = new OldGlobalRates();

            decimal rate = gRates.GetRate(Currency.EUR, Currency.USD);

            Assert.True(rate > 0);
        }

        [Test]
        public void GlobalRatesUpdatePairOK()
        {
            IOldGlobalRates gRates = new OldGlobalRates();

            gRates.UpdatePair(Currency.CAD, Currency.JPY);

            decimal rate = gRates.GetRate(Currency.CAD, Currency.JPY);

            Assert.True(rate > 0);
        }
    }
}
