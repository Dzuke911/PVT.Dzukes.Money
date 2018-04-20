using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business.Tests.MoneyTests
{
    [TestFixture]
    class CommissionAccountTests
    {
        [Test]
        public void AccountSubscribeToCommissionFail()
        {
            //Arrange
            CommissionAccount acc = new CommissionAccount("123", 10, Currency.AUD);

            //Assert
            AccountManager nothing = null;
            var ex = Assert.Throws<ArgumentNullException>(() => acc.SubscribeToCommission(nothing));
            Assert.AreEqual($"{ExceptionMessages.AccountSubscribeNullParam() + Environment.NewLine}Parameter name: accountManager", ex.Message);
        }

        [Test]
        public void AccountUnsubscribeFromCommissionFail()
        {
            //Arrange
            CommissionAccount acc = new CommissionAccount("123", 10, Currency.AUD);

            //Assert
            AccountManager nothing = null;
            var ex = Assert.Throws<ArgumentNullException>(() => acc.UnsubscribeFromCommission(nothing));
            Assert.AreEqual($"{ExceptionMessages.AccountUnsubscribeNullParam() + Environment.NewLine}Parameter name: accountManager", ex.Message);
        }

        [Test]
        public void AccountCheckComission()
        {
            //Arrange
            AccountManager accountManager = new AccountManager(0.01m, Currency.CHF);

            Account accFrom = new Account("from", 547.345m, Currency.USD);
            Account accTo = new Account("to", 56.234m, Currency.EUR);

            CommissionAccount commissionAccount = new CommissionAccount("our money", Currency.CHF);
            commissionAccount.SubscribeToCommission(accountManager);

            //Act
            accountManager.Transaction(accFrom, accTo, 78.12m);

            //Assert
            Assert.AreEqual(0.771934968m, commissionAccount.Money.Amount);
        }
    }
}
