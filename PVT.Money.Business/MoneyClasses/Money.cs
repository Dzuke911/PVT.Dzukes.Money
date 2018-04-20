using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business
{
    [Serializable]
    public sealed class Money
    {
        public decimal Amount { get; private set; }
        public Currency Curr { get; private set; }

        [NonSerialized]
        private ExchangeRates exchangeRates;

        public Money(decimal amount, Currency curr)
        {
            if (amount < 0)
                throw new ArgumentException(ExceptionMessages.MoneyAmountNotLessZero());
            Amount = amount;
            Curr = curr;
            exchangeRates = new ExchangeRates();
        }

        public void Convert(Currency curr)
        {
            Amount *= exchangeRates.GetRate(Curr, curr);
            Curr = curr;
        }

        public void AddMoney(Money mon)
        {
            if (mon == null)
                throw new ArgumentException(ExceptionMessages.MoneyAdditionNullFail());

            Money buffer = new Money(mon.Amount, mon.Curr);
            buffer.AdjustExchangeRate(mon.Curr, Curr, GetExchangeRate(mon.Curr,Curr));
            buffer.Convert(Curr);
            Amount += buffer.Amount;
        }

        public void SubtractMoney(Money mon)
        {
            if (mon == null)
                throw new ArgumentException(ExceptionMessages.MoneySubtractionNullFail());
            if (IsEnoughMoney(mon))
            {
                Amount -= mon.Amount / exchangeRates.GetRate(Curr, mon.Curr);
            }
            else
                throw new InvalidOperationException(ExceptionMessages.MoneySubtractionLessThanSubtractedFail());
        }

        public bool IsEnoughMoney(Money mon, Money mon2, Money mon3)
        {
            decimal sum = 0;

            if (mon == null || mon2 == null || mon3 == null)
                throw new ArgumentException(ExceptionMessages.MoneyComparisonWithNullFail());

            sum += mon2.Amount / exchangeRates.GetRate(Curr, mon2.Curr);
            sum += mon3.Amount / exchangeRates.GetRate(Curr, mon3.Curr);
            sum += mon.Amount / exchangeRates.GetRate(Curr, mon.Curr);

            if (Amount < sum)
                return false;
            else
                return true;
        }

        public bool IsEnoughMoney(Money mon, Money mon2)
        {
            return IsEnoughMoney(mon, mon2, new Money(0, Curr));
        }

        public bool IsEnoughMoney(Money mon)
        {
            return IsEnoughMoney(mon, new Money(0, Curr), new Money(0, Curr));
        }

        public void AdjustExchangeRate(Currency currFrom, Currency currTo, decimal coefficient)
        {
            if (currFrom != currTo)
                exchangeRates.AdjustExchangeRate(currFrom, currTo, coefficient);
        }

        public decimal GetExchangeRate(Currency currFrom, Currency currTo)
        {
            if (currFrom == currTo)
                return 1;
            else
                return exchangeRates.GetRate(currFrom, currTo);
        }
    }
}