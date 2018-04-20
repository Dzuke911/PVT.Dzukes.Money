using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PVT.Money.Business.Enums;

namespace PVT.Money.Business.Tests.MoneyTests
{
    [TestFixture]
    public class AccountManagerTests
    {
        [TestCase(-436)]
        [TestCase(1.001)]
        public void AccountManagerConstructorFail(decimal nominal)
        {
            //Assert
            var ex = Assert.Throws<ArgumentException>(() => new AccountManager(nominal, Currency.AUD));
            Assert.AreEqual(ExceptionMessages.AccountManagerComissionNotPercent(), ex.Message);
        }

        [Test]
        public void AccountManagerConstructorOK()
        {
            //Act
            AccountManager accM = new AccountManager(0.01m, Currency.AUD);

            //Assert
            Assert.NotNull(accM);
        }

        [Test]
        public void AccountManagerConversionOK()
        {
            //Arrange
            AccountManager accM = new AccountManager(0.01m, Currency.USD);

            Account acc1 = new Account("1", 10, Currency.USD);
            Account acc2 = new Account("2", 20, Currency.GBP);
            Account acc3 = new Account("3", 5000, Currency.JPY);

            CommissionAccount commissionAccount = new CommissionAccount("ourMoney", Currency.USD);
            commissionAccount.SubscribeToCommission(accM);

            //Act
            accM.Conversion(acc1, Currency.EUR);
            accM.Conversion(acc2, Currency.AUD);
            accM.Conversion(acc3, Currency.USD);

            //Assert
            Assert.AreEqual(0.808618m, commissionAccount.Money.Amount);
        }

        [Test]
        public void AccountManagerTransactionFromUSDtoEUR()
        {     
            //Arrange
            AccountManager accountManager = new AccountManager(0.01m, Currency.CHF);
            
            Account accFrom = new Account("from", 547.345m, Currency.USD);
            Account accTo = new Account("to",56.234m, Currency.EUR);

            //Act
            accountManager.Transaction(accFrom, accTo, 78.12m);

            //Assert
            Assert.AreEqual(468.4438m, accFrom.Money.Amount);
            Assert.AreEqual(122.0860352m, accTo.Money.Amount);
        }

        public static IEnumerable<TestCaseData> TransactionNullArgumentFail
        {
            get
            {
                Account initialisedAccount = new Account("initialisedAccount", Currency.AUD);
                yield return new TestCaseData(null, initialisedAccount);
                yield return new TestCaseData(initialisedAccount, null);
            }
        }

        [TestCaseSource("TransactionNullArgumentFail")]
        public void TransactionNullAccountFail2Args(Account acc1, Account acc2 )
        { 
            //Arrange
            Account acc = new Account("1", 1m , Currency.USD);
            AccountManager ourManager = new AccountManager(1m, Currency.USD);

            //Assert
            var ex = Assert.Throws<ArgumentException>(() => ourManager.Transaction(acc1, acc2, 1));
            Assert.AreEqual(ExceptionMessages.TransactionAccIsNull(), ex.Message);
        }

        [TestCaseSource("TransactionNullArgumentFail")]
        public void TransactionNullAccountFail3Args(Account acc1, Account acc2)
        {
            //Arrange
            Account acc = new Account("1", 1m, Currency.USD);
            AccountManager ourManager = new AccountManager(1m, Currency.USD);

            //Assert
            var ex = Assert.Throws<ArgumentException>(() => ourManager.Transaction(acc1, acc2, 1, Currency.AUD));
            Assert.AreEqual(ExceptionMessages.TransactionAccIsNull(), ex.Message);
        }

        [TestCase(10, 10.001, 0)]
        [TestCase(10, 8 ,0.5)]
        public void AccountManagerTransactionNotEnoughMoneyFail(decimal amountFrom, decimal transAmount, decimal comission)
        {
            //Arrange
            AccountManager ourManager = new AccountManager(comission, Currency.USD);
            Account accFrom = new Account("from", amountFrom, Currency.USD);
            Account accTo = new Account("to", 123, Currency.EUR);

            //Assert
            var ex = Assert.Throws<InvalidOperationException>(() => ourManager.Transaction(accFrom, accTo, transAmount));
            Assert.AreEqual(ExceptionMessages.TransactionInsufficientFunds(), ex.Message);
        }
    }
}
