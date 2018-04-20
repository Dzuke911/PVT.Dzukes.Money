using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business
{
    public class Rate
    {
        public Currency Curr1 { get; }
        public Currency Curr2 { get; }
        public decimal Coefficient { get; set; }

        public Rate(Currency curr1, Currency curr2, decimal coefficient)
        {
            if (coefficient <= 0)
                throw new ArgumentException(ExceptionMessages.RateNotLessZero());
            Curr1 = curr1;
            Curr2 = curr2;
            Coefficient = coefficient;
        }
    }
}
