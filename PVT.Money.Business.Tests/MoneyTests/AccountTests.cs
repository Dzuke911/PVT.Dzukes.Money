using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace PVT.Money.Business.Tests.MoneyTests
{
    [TestFixture]
    public class AccountTests
    {
        [Test]
        public void AccountConstructorLessZeroFail()
        {
            //Assert
            var ex = Assert.Throws<ArgumentException>(() => new Account("123213414", -12m ,Currency.AUD));
            Assert.AreEqual(ExceptionMessages.MoneyAmountNotLessZero(), ex.Message);
        }

        [Test]
        public void AccountConstructor3ParamsOK()
        {
            //Arrange
            Account acc = new Account("123123123123", 12, Currency.AUD);

            //Assert
            Assert.NotNull(acc);
        }

        [Test]
        public void AccountConstructor2ParamsOK()
        {
            //Arrange
            Account acc = new Account("123123123123", Currency.AUD);

            //Assert
            Assert.NotNull(acc);
        }
    }
}
