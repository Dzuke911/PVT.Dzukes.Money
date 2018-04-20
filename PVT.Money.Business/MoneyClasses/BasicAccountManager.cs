using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business.MoneyClasses
{
    public class BasicAccountManager : IAccountManager
    {
        protected decimal Commission { get; set; }

        public event EventHandler<Money> CommissionSended;

        public BasicAccountManager()
        {
            Commission = 0;
        }

        public void Conversion(Account acc, Currency CurrencyTo)
        {
            if (acc == null)
                throw new ArgumentException(ExceptionMessages.TransactionAccIsNull());

            Money commission = new Money(acc.Money.Amount * Commission, acc.Money.Curr);
            acc.Money.SubtractMoney(commission);
            SendCommission(commission);
            acc.Money.Convert(CurrencyTo);
        }

        public void Transaction(Account accFrom, Account accTo, decimal transactionAmount, Currency transactionCurrency)
        {
            if (accFrom == null || accTo == null)
                throw new ArgumentException(ExceptionMessages.TransactionAccIsNull());
            Money transaction = new Money(transactionAmount, transactionCurrency);
            Money commission = new Money(transactionAmount * Commission, transactionCurrency);
            if (accFrom.Money.IsEnoughMoney(transaction, commission))
            {
                accFrom.Money.SubtractMoney(transaction);
                accFrom.Money.SubtractMoney(commission);
                SendCommission(commission);
                accTo.Money.AddMoney(transaction);
            }
            else
                throw new InvalidOperationException(ExceptionMessages.TransactionInsufficientFunds());
        }

        private void SendCommission(Money m)
        {
            if (m == null)
                throw new ArgumentNullException(nameof(m), ExceptionMessages.SendComissionParamIsNull());
            CommissionSended?.Invoke(this, m);
        }
    }
}
