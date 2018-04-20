using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.Main
{
    public interface IUserInfoManager
    {
        Task<UserInfo> GetUserInfoAsync(string userName);
        Task SetUserInfoAsync(UserInfo userInfo, string userName);
    }
}
