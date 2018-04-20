using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business.MoneyClasses
{
    public class AccountContext
    {
        public AccountContext(IAccountManager manager)
        {
            Manager = manager;
        }

        public IAccountManager Manager { get; set; }

        public void Transaction(Account accFrom, Account accTo, decimal transactionAmount, Currency transactionCurrency)
        {
            Manager.Transaction(accFrom, accTo, transactionAmount, transactionCurrency);
        }

        public void Conversion(Account acc, Currency CurrencyTo)
        {
            Manager.Conversion(acc, CurrencyTo);
        }
    }
}
