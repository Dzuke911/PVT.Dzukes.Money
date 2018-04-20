using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PVT.Money.Business
{
    public class ExchangeRates
    {
        protected List<Rate> _rates;

        public decimal GetRate(Currency currFrom, Currency currTo)
        {
            if (currFrom == currTo)
                return 1m;

            foreach (Rate c in _rates)
            {
                if (c.Curr1 == currFrom && c.Curr2 == currTo)
                    return c.Coefficient;
            }
            throw new InvalidOperationException(ExceptionMessages.ExchangeRatesNoFittedRate());
        }

        public void AdjustExchangeRate(Currency currFrom, Currency currTo, decimal coefficient)
        {
            if (currFrom != currTo)
            {
                Rate rate = _rates.FirstOrDefault(r => r.Curr1 == currFrom && r.Curr2 == currTo);
                if (rate != null)
                    rate.Coefficient = coefficient;
                else
                    _rates.Add(new Rate(currFrom,currTo,coefficient));
            }                
        }

        public ExchangeRates()
        {
            _rates = new List<Rate>();

            //USD Rates
            //_rates.Add(new Rate(Currency.USD, Currency.EUR, 0.84296m));
            //_rates.Add(new Rate(Currency.USD, Currency.GBP, 0.74846m));
            //_rates.Add(new Rate(Currency.USD, Currency.CAD, 1.27199m));
            //_rates.Add(new Rate(Currency.USD, Currency.AUD, 1.29516m));
            //_rates.Add(new Rate(Currency.USD, Currency.CHF, 0.98814m));
            //_rates.Add(new Rate(Currency.USD, Currency.JPY, 113.277m));
            ////EUR Rates
            //_rates.Add(new Rate(Currency.EUR, Currency.USD, 1.18629m));
            //_rates.Add(new Rate(Currency.EUR, Currency.GBP, 0.88787m));
            //_rates.Add(new Rate(Currency.EUR, Currency.CAD, 1.50881m));
            //_rates.Add(new Rate(Currency.EUR, Currency.AUD, 1.53607m));
            //_rates.Add(new Rate(Currency.EUR, Currency.CHF, 1.17225m));
            //_rates.Add(new Rate(Currency.EUR, Currency.JPY, 134.381m));
            ////GBP Rates
            //_rates.Add(new Rate(Currency.GBP, Currency.USD, 1.33614m));
            //_rates.Add(new Rate(Currency.GBP, Currency.EUR, 1.12631m));
            //_rates.Add(new Rate(Currency.GBP, Currency.CAD, 1.69930m));
            //_rates.Add(new Rate(Currency.GBP, Currency.AUD, 1.72987m));
            //_rates.Add(new Rate(Currency.GBP, Currency.CHF, 1.32046m));
            //_rates.Add(new Rate(Currency.GBP, Currency.JPY, 151.354m));
            ////CAD Rates
            //_rates.Add(new Rate(Currency.CAD, Currency.USD, 0.78629m));
            //_rates.Add(new Rate(Currency.CAD, Currency.EUR, 0.66281m));
            //_rates.Add(new Rate(Currency.CAD, Currency.GBP, 0.58848m));
            //_rates.Add(new Rate(Currency.CAD, Currency.AUD, 1.01799m));
            //_rates.Add(new Rate(Currency.CAD, Currency.CHF, 0.77694m));
            //_rates.Add(new Rate(Currency.CAD, Currency.JPY, 89.0689m));
            ////AUD Rates
            //_rates.Add(new Rate(Currency.AUD, Currency.USD, 0.77247m));
            //_rates.Add(new Rate(Currency.AUD, Currency.EUR, 0.65116m));
            //_rates.Add(new Rate(Currency.AUD, Currency.GBP, 0.57813m));
            //_rates.Add(new Rate(Currency.AUD, Currency.CAD, 1.27199m));
            //_rates.Add(new Rate(Currency.AUD, Currency.CHF, 0.76301m));
            //_rates.Add(new Rate(Currency.AUD, Currency.JPY, 87.5037m));
            ////CHF Rates
            //_rates.Add(new Rate(Currency.CHF, Currency.USD, 1.01195m));
            //_rates.Add(new Rate(Currency.CHF, Currency.EUR, 0.85303m));
            //_rates.Add(new Rate(Currency.CHF, Currency.GBP, 0.75737m));
            //_rates.Add(new Rate(Currency.CHF, Currency.CAD, 1.28704m));
            //_rates.Add(new Rate(Currency.CHF, Currency.AUD, 1.31016m));
            //_rates.Add(new Rate(Currency.CHF, Currency.JPY, 114.637m));
            ////JPY Rates
            //_rates.Add(new Rate(Currency.JPY, Currency.USD, 0.0088278m));
            //_rates.Add(new Rate(Currency.JPY, Currency.EUR, 0.0074415m));
            //_rates.Add(new Rate(Currency.JPY, Currency.GBP, 0.0066070m));
            //_rates.Add(new Rate(Currency.JPY, Currency.CAD, 0.0112275m));
            //_rates.Add(new Rate(Currency.JPY, Currency.AUD, 0.0114295m));
            //_rates.Add(new Rate(Currency.JPY, Currency.CHF, 0.0087232m));
        }
    }
}
