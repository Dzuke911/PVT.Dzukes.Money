using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business.MoneyClasses
{
    public class CommissionBank
    {
        private CommissionAccount commissionAcc;

        private CommissionBank()
        {
            commissionAcc = new CommissionAccount("commAcc", 0, Currency.USD);
        }

        private static readonly Lazy<CommissionBank> lazy = new Lazy<CommissionBank>(() => new CommissionBank());

        public static CommissionBank Bank
        {
            get
            {
                return lazy.Value;
            }
        }

        public void Subscribe(AccountManager accManager)
        {
            commissionAcc.SubscribeToCommission(accManager);
        }

        public Money GetMoney()
        {
            return commissionAcc.Money;
        }
    }
}
