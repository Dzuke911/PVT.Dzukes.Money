using PVT.Money.Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PVT.Money.Shell.Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserName(this ClaimsPrincipal cp)
        {
            return cp.Identity.Name;
        }

        public static bool IsInPermission(this ClaimsPrincipal cp, RolePermissions permission)
        {
            return cp.HasClaim("PermissionClaim", permission.ToString());
        }
    }
}
