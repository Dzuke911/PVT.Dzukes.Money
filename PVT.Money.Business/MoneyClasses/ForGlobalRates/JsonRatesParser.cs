using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business.MoneyClasses.ForGlobalRates
{
    public static class JsonRatesParser
    {
        public static List<Rate> FromFixerIO(string str)
        {
            decimal rate;
            List<Rate> result = new List<Rate>();

            dynamic answers = JObject.Parse(str);

            string fromCurrName = answers["base"];
            Currency currentCurr = fromCurrName.StrToCurrency();

            dynamic strRates = answers["rates"];

            foreach (Currency c in (new List<Currency>() { Currency.AUD, Currency.CAD, Currency.CHF, Currency.EUR, Currency.GBP, Currency.JPY, Currency.USD }))
            {
                if (c != currentCurr)
                {
                    rate = strRates[c.CurrToString()];
                    if (rate > 0)
                        result.Add(new Rate(currentCurr, c, rate));
                }
            }

            return result;
        }

        public static List<Rate> FromCurrencyLayerCom(string str)
        {
            decimal rate;
            List<Rate> result = new List<Rate>();

            dynamic answers = JObject.Parse(str);

            string fromCurrName = answers["source"];
            Currency currentCurr = fromCurrName.StrToCurrency();

            dynamic strRates = answers["quotes"];

            foreach (Currency c in (new List<Currency>() { Currency.AUD, Currency.CAD, Currency.CHF, Currency.EUR, Currency.GBP, Currency.JPY, Currency.USD }))
            {
                if (c != currentCurr)
                {
                    rate = strRates[currentCurr.CurrToString() + c.CurrToString()];
                    if (rate > 0)
                        result.Add(new Rate(currentCurr, c, rate));
                }
            }

            return result;
        }

        public static List<Rate> FromFreeCurrencyconverterapiCom(string str)
        {
            decimal rate;
            List<Rate> result = new List<Rate>();

            dynamic answers = JObject.Parse(str);

            dynamic strRates = answers["results"];

            dynamic find = null;

            foreach (Currency c1 in (new List<Currency>() { Currency.AUD, Currency.CAD, Currency.CHF, Currency.EUR, Currency.GBP, Currency.JPY, Currency.USD }))
            {
                foreach (Currency c2 in (new List<Currency>() { Currency.AUD, Currency.CAD, Currency.CHF, Currency.EUR, Currency.GBP, Currency.JPY, Currency.USD }))
                {
                    find = null;
                    if (c1 != c2)
                        find = strRates[c1.CurrToString() +"_"+c2.CurrToString()];
                    if (find!= null)
                    {
                        rate = find["val"];
                        result.Add(new Rate(c1, c2, rate));
                    }                       
                }                    
            }
            return result;
        }
    }
}
