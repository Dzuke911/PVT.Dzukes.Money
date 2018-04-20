using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business.MoneyClasses.ForGlobalRates
{
    public interface IUpdateCurrencyRate
    {
        List<Rate> GetRates(DateTime time);
    }
}
