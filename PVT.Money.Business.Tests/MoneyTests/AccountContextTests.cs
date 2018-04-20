using NUnit.Framework;
using PVT.Money.Business.MoneyClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business.Tests.MoneyTests
{
    [TestFixture]
    public class AccountContextTests
    {
        [Test]
        public void AccountContextOK()
        {
            IAccountManager manager = new AccountManager10percent();
            AccountContext context = new AccountContext(manager);
            Account accFrom = new Account("from", 200m ,Currency.USD);
            Account accTo = new Account("to", 50m, Currency.USD);
            CommissionAccount accCommission = new CommissionAccount("commission", 0m, Currency.USD);
            accCommission.SubscribeToCommission(context.Manager);

            context.Transaction(accFrom, accTo, 100m, Currency.USD);

            Assert.AreEqual(90m, accFrom.Money.Amount);
            Assert.AreEqual(150m, accTo.Money.Amount);
            Assert.AreEqual(10m, accCommission.Money.Amount);
        }
    }
}
