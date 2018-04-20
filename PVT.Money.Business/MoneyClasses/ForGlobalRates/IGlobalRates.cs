using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.MoneyClasses.ForGlobalRates
{
    public interface IGlobalRates
    {
        Task<decimal> GetRateAsync(Currency currFrom, Currency currTo);
    }
}
