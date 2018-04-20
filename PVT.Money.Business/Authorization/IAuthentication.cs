using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.Authorization
{
    public interface IAuthentication
    {
        Task<SignInResult> SignInAsync(string login, string password, bool rememberMe, bool lockoutOnFailure);
        Task SignOutAsync();
        Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync();
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
        Task<SignInResult> ExternalLoginSignInAsync( string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor);
        Task SignInAsync(string userName, bool isPersistent);
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);
        Task<bool> CheckPasswordAsync(string password, string userName);
    }
}
