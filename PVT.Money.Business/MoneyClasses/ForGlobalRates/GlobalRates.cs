using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.MoneyClasses.ForGlobalRates
{
    public class GlobalRates : FCCMethods,IGlobalRates
    {
        public async Task<decimal> GetRateAsync(Currency currFrom, Currency currTo)
        {
            if (currFrom == currTo)
                return 1;

            string strRate = await GetFCCJsonSingleAsync(currFrom,currTo);
            List<Rate> rates = JsonRatesParser.FromFreeCurrencyconverterapiCom(strRate);
            return rates[0].Coefficient;
        }
    }
}
