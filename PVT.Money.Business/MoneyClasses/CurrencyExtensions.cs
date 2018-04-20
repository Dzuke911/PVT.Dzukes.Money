using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business
{
    public static class CurrencyExtensions
    {
        public static string CurrToString(this Currency curr)
        {
            switch (curr)
            {
                case Currency.AUD:
                    return "AUD";
                case Currency.CAD:
                    return "CAD";
                case Currency.CHF:
                    return "CHF";
                case Currency.EUR:
                    return "EUR";
                case Currency.GBP:
                    return "GBP";
                case Currency.JPY:
                    return "JPY";
                case Currency.USD:
                    return "USD";
                default:
                    throw new Exception("Can`t convert unknown currency to string.");
            }
        }

        public static Currency StrToCurrency(this string str)
        {
            switch (str)
            {
                case "AUD":
                    return Currency.AUD;
                case "CAD":
                    return Currency.CAD;
                case "CHF":
                    return Currency.CHF;
                case "EUR":
                    return Currency.EUR;
                case "GBP":
                    return Currency.GBP;
                case "JPY":
                    return Currency.JPY;
                case "USD":
                    return Currency.USD;
                default:
                    throw new Exception("Can`t convert unknown string to currency.");
            }
        }
    }
}
