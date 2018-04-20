using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using PVT.Money.Business.MoneyClasses.ForGlobalRates;

namespace PVT.Money.Business.MoneyClasses
{
    public class OldGlobalRates : IOldGlobalRates
    {
        protected List<Rate> _rates;
        protected DateTime _updateTime;
        readonly TimeSpan _updateDifference;

        public OldGlobalRates()
        {
            _updateDifference = new TimeSpan(0, 5, 0);
            _rates = new List<Rate>();
            UpdateRate(Currency.EUR);
            UpdateRate(Currency.USD);
            UpdateRate(Currency.CHF);
            UpdateRate(Currency.AUD);
        }

        public decimal GetRate(Currency currFrom, Currency currTo)
        {
            if (currFrom == currTo)
                return 1m;

            TimeSpan difference = DateTime.Now - _updateTime;

            if (difference >= _updateDifference)
            {
                UpdateRate(currFrom);
            }

            foreach (Rate c in _rates)
            {
                if (c.Curr1 == currFrom && c.Curr2 == currTo)
                    return c.Coefficient;
            }
            throw new InvalidOperationException(ExceptionMessages.ExchangeRatesNoFittedRate());
        }

        public DateTime GetUpdateTime()
        {
            return _updateTime;
        }

        private void UpdateRate(Currency curr)
        {
            IUpdateCurrencyRate updater = null;
            List<Rate> rates = new List<Rate>();

            switch (curr)
            {
                case Currency.EUR:
                    updater = new UpdateEURRate();
                    break;
                case Currency.USD:
                    updater = new UpdateUSDRate();
                    break;
                case Currency.CHF:
                    updater = new UpdateCHFRate();
                    break;
                case Currency.AUD:
                    updater = new UpdateAUDRate();
                    break;
                default:
                    throw new NotImplementedException($"The UpdateRate method can`t use {curr.CurrToString()} currency yet.");
            }

            if(updater != null)
                rates = updater.GetRates(_updateTime);

            foreach(Rate r in rates)
            {
                AddRate(r.Curr1, r.Curr2, r.Coefficient);
            }
        }

        private void AddRate(Currency currFrom, Currency currTo, decimal rate)
        {
            foreach (Rate r in _rates)
            {
                if (r.Curr1 == currFrom && r.Curr2 == currTo)
                {
                    r.Coefficient = rate;
                    return;
                }
            }
            _rates.Add(new Rate(currFrom, currTo, rate));
        }

        public void UpdatePair(Currency curr1, Currency curr2)
        {
            List<Rate> rates = new List<Rate>();
            string url = $"https://free.currencyconverterapi.com/api/v5/convert?q={curr1.CurrToString()}_{curr2.CurrToString()},{curr2.CurrToString()}_{curr1.CurrToString()}";

            using (HttpClient client = new HttpClient())
            {
                Task<HttpResponseMessage> task1 = client.GetAsync(url);
                HttpResponseMessage res = task1.Result;
                HttpContent content = res.Content;
                Task<string> task2 = content.ReadAsStringAsync();
                _updateTime = DateTime.Now;
                rates = JsonRatesParser.FromFreeCurrencyconverterapiCom(task2.Result);
            }

            foreach (Rate r in rates)
            {
                AddRate(r.Curr1, r.Curr2, r.Coefficient);
            }
        }
    }
}
