using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.MoneyClasses.ForGlobalRates
{
    class UpdateCHFRate : FCCMethods,IUpdateCurrencyRate
    {
        public List<Rate> GetRates(DateTime time)
        {
            List<Rate> result = JsonRatesParser.FromFreeCurrencyconverterapiCom(GetFCCJson(Currency.CHF,Currency.AUD,Currency.CHF,Currency.CAD));
            result.AddRange(JsonRatesParser.FromFreeCurrencyconverterapiCom(GetFCCJson(Currency.CHF, Currency.EUR, Currency.CHF, Currency.GBP)));
            result.AddRange(JsonRatesParser.FromFreeCurrencyconverterapiCom(GetFCCJson(Currency.CHF, Currency.JPY, Currency.CHF, Currency.USD)));

            time = DateTime.Now;
            return result;
        }
    }
}
