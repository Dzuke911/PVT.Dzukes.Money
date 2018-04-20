using Microsoft.AspNetCore.Identity;
using PVT.Money.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<ApplicationUser> GetUserAsync(this UserManager<ApplicationUser> um , string name)
        {
            ApplicationUser user = await um.FindByNameAsync(name);

            if (user == null)
                throw new InvalidOperationException($"There is no '{name}' user in database.");

            return user;
        }
    }
}
