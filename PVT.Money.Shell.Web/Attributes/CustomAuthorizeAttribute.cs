using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using PVT.Money.Business.Admin;
using PVT.Money.Business.Enums;
using Microsoft.AspNetCore.Mvc;
using PVT.Money.Business.Main;

namespace PVT.Money.Shell.Web.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Method , AllowMultiple = true, Inherited = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        readonly RolePermissions _permission;

        public CustomAuthorizeAttribute(RolePermissions permission)
        {
            _permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            IPermissionManager permissionManager = context.HttpContext.RequestServices.GetService<IPermissionManager>();
            IMoneyUserManager userManager = context.HttpContext.RequestServices.GetService<IMoneyUserManager>();

            string roleName = userManager.GetUserRole(context.HttpContext.User.Identity.Name);

            if(!permissionManager.IsRoleHavePermission(roleName, _permission))
                context.Result = new UnauthorizedResult();
        }
    }
}
