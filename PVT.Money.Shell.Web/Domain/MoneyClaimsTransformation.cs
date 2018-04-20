using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using PVT.Money.Business.Admin;
using PVT.Money.Business.Main;

namespace PVT.Money.Shell.Web.Domain
{
    public class MoneyClaimsTransformation : IClaimsTransformation
    {
        IMoneyUserManager _moneyUserManager;
        IPermissionManager _permissionManager;

        public MoneyClaimsTransformation(IMoneyUserManager moneyUserManager, IPermissionManager permissionManager)
        {
            _moneyUserManager = moneyUserManager;
            _permissionManager = permissionManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            string roleName;
            try
            {
                roleName = _moneyUserManager.GetUserRole(principal.Identity.Name);
            }
            catch
            {
                return principal;
            }
            
            IEnumerable<Claim> claims = await _permissionManager.GetRolePermissionsAsClaimsAsync(roleName);
            principal.AddIdentity(new ClaimsIdentity(claims));

            return principal;
        }
    }
}
