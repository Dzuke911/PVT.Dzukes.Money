using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.MoneyClasses.ForGlobalRates
{
    class UpdateEURRate : IUpdateCurrencyRate
    {
        public List<Rate> GetRates(DateTime time)
        {
            string url = $"http://data.fixer.io/api/latest?access_key=790f080c50e32e7a208b637a6c29623f&base=EUR&symbols=AUD,CAD,CHF,GBP,JPY,USD,EUR";
            //string url = $"http://www.nbrb.by/API/ExRates/Rates/145";

            using (HttpClient client = new HttpClient())
            {
                Task<HttpResponseMessage> task1 = client.GetAsync(url);
                HttpResponseMessage res = task1.Result;
                HttpContent content = res.Content;
                Task<string> task2 = content.ReadAsStringAsync();
                time = DateTime.Now;
                return JsonRatesParser.FromFixerIO(task2.Result);
            }
        }
    }
}
