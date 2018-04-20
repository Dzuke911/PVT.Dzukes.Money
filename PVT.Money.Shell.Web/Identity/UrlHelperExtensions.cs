using Microsoft.AspNetCore.Mvc;
using PVT.Money.Shell.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PVT.Money.Shell.Web.Identity
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            //return urlHelper.Action(
            //    action: nameof(AccountController.ConfirmEmail),
            //    controller: "Account",
            //    values: new { userId, code },
            //    protocol: scheme);
            return "";
        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, int userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ResetPassword),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }
    }
}
