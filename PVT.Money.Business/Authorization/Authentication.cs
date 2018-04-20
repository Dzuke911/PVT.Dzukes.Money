using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PVT.Money.Business.Enums;
using PVT.Money.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PVT.Money.Business.Authorization;
using Microsoft.AspNetCore.Authentication;
using PVT.Money.Business.Extensions;

namespace PVT.Money.Business
{
    public class Authentication : IAuthentication
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        internal Authentication(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<SignInResult> SignInAsync(string login, string password, bool rememberMe, bool lockoutOnFailure)
        {
            SignInResult result = null;

            ApplicationUser emailUser = await _userManager.FindByEmailAsync(login);

            if(emailUser != null)
                result = await _signInManager.PasswordSignInAsync(emailUser.UserName, password, rememberMe, lockoutOnFailure);

            if (result != SignInResult.Success)
            {
                return await _signInManager.PasswordSignInAsync(login, password, rememberMe, lockoutOnFailure);
            }
            else
                return result;
        }

        public async Task SignInAsync(string userName, bool isPersistent)
        {
            ApplicationUser user = await _userManager.GetUserAsync(userName);

            await _signInManager.SignInAsync(user, false);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public bool CheckRole(string role, MoneyUser user)
        {
            return user.RoleName == role;
        }

        public async Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync()
        {
            return await _signInManager.GetExternalAuthenticationSchemesAsync();
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
           return await _signInManager.GetExternalLoginInfoAsync();
        }

        public async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor)
        {
            return await _signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, bypassTwoFactor);
        }

        public async Task<bool> CheckPasswordAsync(string password, string userName)
        {
            ApplicationUser user = await _userManager.GetUserAsync(userName);

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded)
                return true;
            else
                return false;
        }
    }
}
