using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.Main
{
    public interface IMoneyUserManager
    {
        Task<IEnumerable<MoneyUser>> GetUsersAsync();
        Task<string> GetUserEmailAsync(string UserName);
        Task<string> GetUserNameAsync(string nameOrEmail);
        Task<string> GetUserNameAsync(string loginProvider, string providerKey);
        Task<IdentityResult> AddLoginAsync(string user, ExternalLoginInfo info);
        Task<string> GeneratePasswordResetTokenAsync(string email);
        Task<int> GetUserIdAsync(string email);
        Task<IdentityResult> ChangePasswordAsync(string currentPassword, string newPassword, string userName);
        Task<bool> IsNotOAuth(string userName);
        Task<string> CreatePasswordAsync(string userNameOrLogin, string password, string code);

        string GetUserRole(string userName);
    }
}
