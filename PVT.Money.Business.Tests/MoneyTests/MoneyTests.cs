using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using PVT.Money.Business;

namespace PVT.Money.Business.Tests.MoneyTests
{
    [TestFixture]
    class MoneyTests
    {
        [Test]
        public void MoneyConstructorFail()
        {
            //Assert
            Assert.Throws<ArgumentException>(() =>new Money(-0.5m,Currency.AUD));
        }

        [Test]
        public void MoneyConstructorOK()
        {
            //Arrange
            Money money = new Money(100, Currency.AUD);

            //Assert
            Assert.NotNull(money);
        }

        [TestCase(Currency.USD, Currency.EUR, 10, 8.4296)]
        [TestCase(Currency.EUR, Currency.GBP, 15, 13.31805)]
        [TestCase(Currency.GBP, Currency.CAD, 4, 6.7972)]
        [TestCase(Currency.CAD, Currency.AUD, 123, 125.21277)]
        [TestCase(Currency.AUD, Currency.CHF, 71.54, 54.5857354)]
        [TestCase(Currency.CHF, Currency.JPY, 0.55, 63.05035)]
        [TestCase(Currency.JPY, Currency.USD, 75.24, 0.664203672)]
        public void Conversion(Currency currFrom, Currency currTo, decimal amount,  decimal result)
        {
            //Arrange
            Money money = new Money(amount, currFrom);

            //Act
            money.Convert(currTo);

            //Assert
            Assert.AreEqual(money.Amount, result);
            Assert.AreEqual(money.Curr, currTo);
        }
        
        [Test]
        public void AddMoneyNullFail()
        {
            //Arrange
            Money money = new Money(1, Currency.AUD);

            //Assert
            var ex = Assert.Throws<ArgumentException>(() => money.AddMoney(null));
            Assert.AreEqual(ExceptionMessages.MoneyAdditionNullFail(), ex.Message);
        }

        [TestCase(5,Currency.EUR,10,Currency.USD, 13.4296)]
        public void AddMoneyOK(decimal amountTo, Currency currTo, decimal amountAdd, Currency currAdd, decimal result)
        {
            //Arrange
            Money moneyTo = new Money(amountTo, currTo);
            Money moneyAdd = new Money(amountAdd, currAdd);

            //Act
            moneyTo.AddMoney(moneyAdd);

            //Assert
            Assert.AreEqual(result, moneyTo.Amount);
        }

        [Test]
        public void SubtractMoneyNullFail()
        {
            //Arrange
            Money money = new Money(1, Currency.AUD);

            //Assert
            var ex = Assert.Throws<ArgumentException>(() => money.SubtractMoney(null));
            Assert.AreEqual(ExceptionMessages.MoneySubtractionNullFail(), ex.Message);
        }

        [TestCase(8.42955, Currency.EUR, 10, Currency.USD)]
        public void SubtractMoneyNotEnoughFail(decimal amountFrom, Currency currFrom, decimal amountSub, Currency currSub)
        {
            //Arrange
            Money moneyFrom = new Money(amountFrom, currFrom);
            Money moneySub = new Money(amountSub, currSub);

            //Assert
            var ex = Assert.Throws<InvalidOperationException>(() => moneyFrom.SubtractMoney(moneySub));
            Assert.AreEqual(ExceptionMessages.MoneySubtractionLessThanSubtractedFail(), ex.Message);
        }

        [TestCase( 1 , Currency.EUR, 1.18629, Currency.USD, 0)]
        public void SubtractMoneyOK(decimal amountFrom, Currency currFrom, decimal amountSub, Currency currSub, decimal result)
        {
            //Arrange
            Money moneyFrom = new Money(amountFrom, currFrom);
            Money moneySub = new Money(amountSub, currSub);

            //Act
            moneyFrom.SubtractMoney(moneySub);

            //Assert
            Assert.AreEqual(result, moneyFrom.Amount);
        }

        [Test]
        public void IsEnoughMoney1ArgNullFail()
        {
            //Arrange
            Money money = new Money(1, Currency.AUD);

            //Assert
            var ex = Assert.Throws<ArgumentException>(() => money.IsEnoughMoney(null));
            Assert.AreEqual(ExceptionMessages.MoneyComparisonWithNullFail(), ex.Message);
        }

        public static IEnumerable<TestCaseData> IsEnoughMoney2ArgsNullTestCases
        { 
            get
            {
                Money initialisedMoney = new Money(10, Currency.AUD);
                yield return new TestCaseData(initialisedMoney, null);
                yield return new TestCaseData(null, initialisedMoney);
            }
        }

        [TestCaseSource("IsEnoughMoney2ArgsNullTestCases")]
        public void IsEnoughMoney2ArgsNullFail(Money money1, Money money2)
        {
            //Arrange
            Money money = new Money(1, Currency.AUD);

            //Assert
            var ex = Assert.Throws<ArgumentException>(() => money.IsEnoughMoney(money1,money2));
            Assert.AreEqual(ExceptionMessages.MoneyComparisonWithNullFail(), ex.Message);
        }

        public static IEnumerable<TestCaseData> IsEnoughMoney3ArgsNullTestCases
        {
            get
            {
                Money initialisedMoney = new Money(10, Currency.AUD);
                yield return new TestCaseData(null, initialisedMoney, initialisedMoney);
                yield return new TestCaseData(initialisedMoney, null, initialisedMoney);
                yield return new TestCaseData(initialisedMoney, initialisedMoney, null);
            }
        }

        [TestCaseSource("IsEnoughMoney3ArgsNullTestCases")]
        public void IsEnoughMoney3ArgsNullFail(Money money1, Money money2, Money money3)
        {
            //Arrange
            Money money = new Money(1, Currency.AUD);

            //Assert
            var ex = Assert.Throws<ArgumentException>(() => money.IsEnoughMoney(money1, money2,money3));
            Assert.AreEqual(ExceptionMessages.MoneyComparisonWithNullFail(), ex.Message);
        }

        [Test]
        public void IsEnoughMoney1ArgTrue()
        {
            //Arrange
            Money moneyFrom = new Money(1, Currency.AUD);
            Money moneySub = new Money( 0.5m, Currency.USD);

            //Assert
            Assert.IsTrue(moneyFrom.IsEnoughMoney(moneySub));
        }

        [Test]
        public void IsEnoughMoney1ArgFalse()
        {
            //Arrange
            Money moneyFrom = new Money(1, Currency.AUD);
            Money moneySub = new Money(1.1m, Currency.USD);

            //Assert
            Assert.IsFalse(moneyFrom.IsEnoughMoney(moneySub));
        }

        [Test]
        public void IsEnoughMoney2ArgsTrue()
        {
            //Arrange
            Money moneyFrom = new Money(1.2m, Currency.AUD);
            Money moneySub1 = new Money(0.45m, Currency.USD);
            Money moneySub2 = new Money(0.35m, Currency.EUR);

            //Assert
            Assert.IsTrue(moneyFrom.IsEnoughMoney(moneySub1,moneySub2));
        }

        [Test]
        public void IsEnoughMoney2ArgsFalse()
        {
            //Arrange
            Money moneyFrom = new Money(1, Currency.AUD);
            Money moneySub1 = new Money(0.5m, Currency.USD);
            Money moneySub2 = new Money(0.6m, Currency.EUR);

            //Assert
            Assert.IsFalse(moneyFrom.IsEnoughMoney(moneySub1, moneySub2));
        }

        [Test]
        public void IsEnoughMoney3ArgsTrue()
        {
            //Arrange
            Money moneyFrom = new Money(1.3m, Currency.AUD);
            Money moneySub1 = new Money(0.45m, Currency.USD);
            Money moneySub2 = new Money(0.35m, Currency.EUR);
            Money moneySub3 = new Money(0.1m, Currency.GBP);

            //Assert
            Assert.IsTrue(moneyFrom.IsEnoughMoney(moneySub1, moneySub2,moneySub3));
        }

        [Test]
        public void IsEnoughMoney3ArgsFalse()
        {
            //Arrange
            Money moneyFrom = new Money(1.2m, Currency.AUD);
            Money moneySub1 = new Money(0.45m, Currency.USD);
            Money moneySub2 = new Money(0.25m, Currency.EUR);
            Money moneySub3 = new Money(0.2m, Currency.GBP);

            //Assert
            Assert.IsFalse(moneyFrom.IsEnoughMoney(moneySub1, moneySub2, moneySub3));
        }
    }
}
