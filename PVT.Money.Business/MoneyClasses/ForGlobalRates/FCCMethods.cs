using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.MoneyClasses.ForGlobalRates
{
    public class FCCMethods
    {
        protected string GetFCCJson(Currency curr1, Currency curr2, Currency curr3, Currency curr4)
        {
            string url = $"https://free.currencyconverterapi.com/api/v5/convert?q={curr1.CurrToString()}_{curr2.CurrToString()},{curr3.CurrToString()}_{curr4.CurrToString()}";

            using (HttpClient client = new HttpClient())
            {
                Task<HttpResponseMessage> task1 = client.GetAsync(url);
                HttpResponseMessage res = task1.Result;
                Task<string> task2 = res.Content.ReadAsStringAsync();
                return task2.Result;
            }
        }

        protected async Task<string> GetFCCJsonSingleAsync(Currency curr1, Currency curr2)
        {
            string url = $"https://free.currencyconverterapi.com/api/v5/convert?q={curr1.CurrToString()}_{curr2.CurrToString()}";

            using (HttpClient client = new HttpClient())
            {
                Task<HttpResponseMessage> task1 = client.GetAsync(url);
                HttpResponseMessage res = task1.Result;
                return await res.Content.ReadAsStringAsync();
            }
        }
    }
}
