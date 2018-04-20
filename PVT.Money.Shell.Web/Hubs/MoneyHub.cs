using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PVT.Money.Shell.Web.Hubs
{
    public class MoneyHub : Hub
    {
        public void Receive(string message)
        {
            Clients.All.InvokeAsync("Send", message);
        }
    }
}
