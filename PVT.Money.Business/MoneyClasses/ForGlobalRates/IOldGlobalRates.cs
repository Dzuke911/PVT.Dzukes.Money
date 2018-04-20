using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business.MoneyClasses
{
    public interface IOldGlobalRates
    {
        DateTime GetUpdateTime();
        decimal GetRate(Currency currFrom, Currency currTo);
        void UpdatePair(Currency curr2, Currency curr1);
    }
}
