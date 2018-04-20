using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business.MoneyClasses.ForGlobalRates
{
    class UpdateAUDRate : FCCMethods, IUpdateCurrencyRate
    {
        public List<Rate> GetRates(DateTime time)
        {
            List<Rate> result = JsonRatesParser.FromFreeCurrencyconverterapiCom(GetFCCJson(Currency.AUD, Currency.CHF, Currency.AUD, Currency.CAD));
            result.AddRange(JsonRatesParser.FromFreeCurrencyconverterapiCom(GetFCCJson(Currency.AUD, Currency.EUR, Currency.AUD, Currency.GBP)));
            result.AddRange(JsonRatesParser.FromFreeCurrencyconverterapiCom(GetFCCJson(Currency.AUD, Currency.JPY, Currency.AUD, Currency.USD)));

            time = DateTime.Now;
            return result;
        }
    }
}
