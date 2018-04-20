using NUnit.Framework;
using PVT.Money.Business.MoneyClasses.ForGlobalRates;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.Tests.MoneyTests
{
    [TestFixture]
    public class GlobalRatesTest
    {
        [Test]
        public async Task GetRateTest()
        {
            IGlobalRates gr = new GlobalRates();
            decimal coeff = await gr.GetRateAsync(Currency.USD, Currency.EUR);
            Assert.IsTrue(coeff > 0);
        }
    }
}
