using NUnit.Framework;
using PVT.Money.Business.MoneyClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business.Tests.MoneyTests
{
    [TestFixture]
    public class CommissionBankTests
    {
        [Test]
        public void TransactionComissionBankOK()
        {
            AccountManager accM = new AccountManager(0.1m, Currency.USD);
            CommissionBank.Bank.Subscribe(accM);
            Account acc1 = new Account("acc1", 200, Currency.USD);
            Account acc2 = new Account("acc2", 0, Currency.USD);

            accM.Transaction(acc1, acc2, 100, Currency.USD);

            Assert.AreEqual(10, CommissionBank.Bank.GetMoney().Amount);
            Assert.AreEqual(100, acc2.Money.Amount);
            Assert.AreEqual(90, acc1.Money.Amount);
        }
    }
}
