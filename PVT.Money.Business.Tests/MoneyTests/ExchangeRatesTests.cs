using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace PVT.Money.Business.Tests.MoneyTests
{
    [TestFixture]
    class ExchangeRatesTests
    {        
        [Test]
        public void ExchangeRatesConstructorOK()
        {
            //Arrange
            ExchangeRates rates = new ExchangeRates();
            
            //Assert
            Assert.NotNull(rates);
        }

        [Test]
        public void AreAllRates()
        {
            //Arrange
            Currency[] currencies = new Currency[] { Currency.AUD, Currency.CAD, Currency.CHF, Currency.EUR, Currency.GBP, Currency.JPY, Currency.USD };
            ExchangeRates rates = new ExchangeRates();
            string res = "";
            decimal rate = 0;

            //Act
            for (int i = 0; i < currencies.Length; i++)
            {
                for (int j = 0; j < currencies.Length; j++)
                {
                    try
                    {
                        rate = rates.GetRate(currencies[i], currencies[j]);
                    }
                    catch
                    {
                        res += currencies[i].ToString() + "-" + currencies[j].ToString() + "|";
                    }
                    if (i == j && rate != 1m)
                            res+= currencies[i].ToString() + "-" + currencies[j].ToString() + "|";
                }
            }

            //Assert
            Assert.AreEqual("",res);
        }
    }
}
