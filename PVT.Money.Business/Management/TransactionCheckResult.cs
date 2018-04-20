using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business.Main
{
    public class TransactionCheckResult
    {
        public bool IsSameAccs { get; set; }
        public bool IsParsed { get; set; }
        public bool IsEnough { get; set; }
        public decimal SendAmount { get; set; }
        public decimal Commission { get; set; }
        public decimal Receive { get; set; }
        public decimal Rest { get; set; }
        public string CurrFrom { get; set; }
        public string CurrTo { get; set; }
        public string CommissionOwner { get; set; }
        public string CommissionAccName { get; set; }
        public int ReqNum { get; set; }
    }
}
