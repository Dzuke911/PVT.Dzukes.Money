using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business.MoneyClasses
{
    public class AccountManager10percent : BasicAccountManager, IAccountManager
    {
        public AccountManager10percent()
        {
            Commission = 0.1m;
        }
    }
}
