using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business.Logger
{
    public class LogEvent
    {
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
    }
}
