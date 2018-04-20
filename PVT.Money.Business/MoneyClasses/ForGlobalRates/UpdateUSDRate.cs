using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.MoneyClasses.ForGlobalRates
{
    class UpdateUSDRate:IUpdateCurrencyRate
    {
        public List<Rate> GetRates(DateTime time)
        {
            string url = $"http://www.apilayer.net/api/live?access_key=1c5acab8d3b855c8d96ccd70bc09e646&source=USD&currencies=GBP,AUD,CAD,CHF,EUR,JPY";

            using (HttpClient client = new HttpClient())
            {
                Task<HttpResponseMessage> task1 = client.GetAsync(url);
                HttpResponseMessage res = task1.Result;
                HttpContent content = res.Content;
                Task<string> task2 = content.ReadAsStringAsync();
                time = DateTime.Now;
                return JsonRatesParser.FromCurrencyLayerCom(task2.Result);
            }
        }
    }
}
