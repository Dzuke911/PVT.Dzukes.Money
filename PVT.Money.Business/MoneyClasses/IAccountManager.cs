using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business.MoneyClasses
{
    public interface IAccountManager
    {
        void Transaction(Account accFrom, Account accTo, decimal transactionAmount, Currency transactionCurrency);

        void Conversion(Account acc, Currency CurrencyTo);

        event EventHandler<Money> CommissionSended;
    }
}
