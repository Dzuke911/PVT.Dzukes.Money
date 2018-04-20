using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business.Tests
{
    public class MoneyOptions : IOptions<IdentityOptions>
    {
        public IdentityOptions Value => new IdentityOptions();

        public MoneyOptions()
        {
           
        }
    }
}
