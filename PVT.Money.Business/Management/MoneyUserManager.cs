using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PVT.Money.Business.Admin;
using PVT.Money.Business.Enums;
using PVT.Money.Business.Logger;
using PVT.Money.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using PVT.Money.Business.Extensions;

namespace PVT.Money.Business.Main
{
    ///////////////////////////////////////
    /// ANTIPATTERN: GOD OBJECT
    ///////////////////////////////////////
    public class MoneyUserManager : IMoneyUserManager
    {
        protected DatabaseContext DBContext;
        private readonly UserManager<ApplicationUser> _userManager;

        internal MoneyUserManager(DatabaseContext dbContext, UserManager<ApplicationUser> userManager)
        {
            DBContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IEnumerable<MoneyUser>> GetUsersAsync()
        {
            List<MoneyUser> users = new List<MoneyUser>();

            IEnumerable<ApplicationUser> userEntities = await _userManager.Users.ToArrayAsync();

            foreach (ApplicationUser u in userEntities)
            {
                IList<string> roles = await _userManager.GetRolesAsync(u);
                users.Add(new MoneyUser
                {
                    Login = u.UserName,
                    Email = u.Email,
                    RoleName = roles[0],
                    Password = ""
                });
            }

            users.Sort((u1, u2) => u1.Login.CompareTo(u2.Login));
            return users;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            if (user == null )
                return null;

            // Для получения дополнительной информации о том, как включить подтверждение учетной записи и сброс пароля, 
            // перейдите на сайт https://go.microsoft.com/fwlink/?LinkID=532713
            return await _userManager.GeneratePasswordResetTokenAsync(user);           
        }

        public async Task<int> GetUserIdAsync(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new InvalidOperationException($"There is no user with '{email}' email in database.");
            return user.Id;
        }

        public async Task<string> GetUserEmailAsync(string UserName)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(UserName);

            return user?.Email;
        }

        public async Task<string> GetUserNameAsync(string nameOrEmail)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(nameOrEmail);
            if (user != null)
                return user.UserName;

            user = await _userManager.FindByNameAsync(nameOrEmail);
            if (user == null)
                throw new InvalidOperationException($"There is no user with '{nameOrEmail}' name or email in database.");
            else
                return user.UserName;
        }

        public async Task<IdentityResult> AddLoginAsync(string userName, ExternalLoginInfo info)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new InvalidOperationException($"There is no '{userName}' user in database.");

            return await _userManager.AddLoginAsync(user, info);
        }

        public async Task<string> GetUserNameAsync(string loginProvider, string providerKey)
        {
            ApplicationUser user = await _userManager.FindByLoginAsync(loginProvider, providerKey);
            if (user == null)
                throw new InvalidOperationException($"Can`t find user registered with '{loginProvider}' provider.");

            return user.UserName;
        }

        public async Task<IdentityResult> ChangePasswordAsync(string currentPassword, string newPassword, string userName)
        {
            ApplicationUser user = await _userManager.GetUserAsync(userName);

            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task<bool> IsNotOAuth(string userName)
        {
            ApplicationUser user = await _userManager.GetUserAsync(userName);

            return await _userManager.HasPasswordAsync(user);
        }

        public async Task<string> CreatePasswordAsync(string userNameOrLogin, string password, string code)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(userNameOrLogin);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(userNameOrLogin);
                if (user == null)
                    return null;
            }
                
            IdentityResult result = await _userManager.ResetPasswordAsync(user, code, password);

            if (result.Succeeded)
            {
                return user.UserName;
            }
            return null;
        }

        public string GetUserRole(string userName)
        {
            Task<ApplicationUser> task = _userManager.FindByNameAsync(userName);
            ApplicationUser user = task.Result;
            if (user == null)
                throw new InvalidOperationException($"There is no '{userName}' user in database.");

            Task<bool> res;

            IEnumerable<ApplicationRole> roles = DBContext.Roles.ToArray();
            foreach(ApplicationRole r in roles)
            {
                res = _userManager.IsInRoleAsync(user, r.Name);
                if (res.Result == true)
                    return r.Name;
            }

            return null;
        }
    }
}
