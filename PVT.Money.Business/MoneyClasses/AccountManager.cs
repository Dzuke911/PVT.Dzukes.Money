using PVT.Money.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business
{
    public class AccountManager
    {

        public decimal Commission { get; }
        public Currency CommissionCurr { get; }

        public event EventHandler<Money> CommissionSended;

        public AccountManager( decimal commission, Currency commissionCurr)
        {
            if (commission < 0 || commission >1)
                throw new ArgumentException( ExceptionMessages.AccountManagerComissionNotPercent());
            Commission = commission;
            CommissionCurr = commissionCurr;  
        }

        public void Conversion( Account acc, Currency CurrencyTo)
        {
            if (acc == null)
                throw new ArgumentException(ExceptionMessages.TransactionAccIsNull());

            Money commission = new Money(acc.Money.Amount * Commission, acc.Money.Curr);
            acc.Money.SubtractMoney(commission);
            SendCommission(commission);
            acc.Money.Convert(CurrencyTo);            
        }

        public void Transaction( Account accFrom, Account accTo, decimal transactionAmount, Currency transactionCurrency )
        {
            if (accFrom == null || accTo == null)
                throw new ArgumentException(ExceptionMessages.TransactionAccIsNull());

            Money transaction = new Money(transactionAmount, transactionCurrency);
            transaction.AdjustExchangeRate(accFrom.Money.Curr,accTo.Money.Curr, accFrom.Money.GetExchangeRate(accFrom.Money.Curr, accTo.Money.Curr));
            Money commission = new Money(transactionAmount * Commission, transactionCurrency);
            commission.AdjustExchangeRate(accFrom.Money.Curr, accTo.Money.Curr, accFrom.Money.GetExchangeRate(accFrom.Money.Curr, accTo.Money.Curr));
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

        public void Transaction(Account accFrom, Account accTo, decimal transactionAmount) //transactionAmountCurrency == accFrom.Money.Currency
        {
            if (accFrom == null || accTo == null)
                throw new ArgumentException(ExceptionMessages.TransactionAccIsNull());
            Transaction(accFrom, accTo, transactionAmount, accFrom.Money.Curr);
        }

        private void SendCommission(Money m)
        {
            if (m == null)
                throw new ArgumentNullException(nameof(m), ExceptionMessages.SendComissionParamIsNull());
            CommissionSended?.Invoke(this, m);
        }
    }
}
