using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PVT.Money.Data
{
    public class ApplicationUser : IdentityUser<int>
    {
        [ForeignKey("UserID")]
        public IEnumerable<AccountEntity> Accounts { get; set; }

        [ForeignKey("UserID")]
        public IEnumerable<LogEventEntity> LogEvents { get; set; }
    }
}
