using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PVT.Money.Business.Extensions;
using PVT.Money.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.Main
{
    ///////////////////////////////////////
    /// ANTIPATTERN: HARD CODE
    ///////////////////////////////////////
    public class UserInfoManager : IUserInfoManager
    {
        protected DatabaseContext DBContext;
        private readonly UserManager<ApplicationUser> _userManager;

        internal UserInfoManager(DatabaseContext dbContext, UserManager<ApplicationUser> userManager)
        {
            DBContext = dbContext;
            _userManager = userManager;
        }

        public async Task<UserInfo> GetUserInfoAsync(string userName)
        {
            ApplicationUser user = await _userManager.GetUserAsync(userName);

            UserInfo result = new UserInfo();

            UserInfoEntity uInfo = await DBContext.UsersInfo.Include(ui => ui.User).FirstOrDefaultAsync(ui => ui.User == user);

            result.Address = uInfo.Address;
            if (uInfo.BirthDate?.Day != null) result.BirthDay = (int)uInfo.BirthDate?.Day;
            if (uInfo.BirthDate?.Month != null) result.BirthMonth = (int)uInfo.BirthDate?.Month;
            result.BirthMonthStr = uInfo.BirthDate?.ToString("MMMM");
            if (uInfo.BirthDate?.Year != null) result.BirthYear = (int)uInfo.BirthDate?.Year;
            result.Email = user.Email;
            result.FirstName = uInfo.FirstName;
            result.Gender = uInfo.Gender;
            result.LastName = uInfo.LastName;
            result.Login = user.UserName;
            result.Phone = uInfo.Phone;

            return result;

        }

        public async Task SetUserInfoAsync(UserInfo userInfo, string userName)
        {
            if (userInfo == null)
                throw new ArgumentNullException("userInfo");
            if (userName == null)
                throw new ArgumentNullException("userName");

            ApplicationUser user = await _userManager.GetUserAsync(userName);

            UserInfo result = new UserInfo();

            UserInfoEntity uInfo = await DBContext.UsersInfo.Include(ui => ui.User).FirstOrDefaultAsync(ui => ui.User == user);

            await _userManager.SetUserNameAsync(user, userInfo.Login);
            await _userManager.SetEmailAsync(user, userInfo.Email);

            uInfo.Address = userInfo.Address;
            uInfo.BirthDate = new DateTime(userInfo.BirthYear, userInfo.BirthMonth, userInfo.BirthDay);
            uInfo.FirstName = userInfo.FirstName;
            uInfo.Gender = userInfo.Gender;
            uInfo.LastName = userInfo.LastName;
            uInfo.Phone = userInfo.Phone;

            await _userManager.UpdateAsync(user);
            DBContext.UsersInfo.Update(uInfo);

            await DBContext.SaveChangesAsync();
        }
    }
}
