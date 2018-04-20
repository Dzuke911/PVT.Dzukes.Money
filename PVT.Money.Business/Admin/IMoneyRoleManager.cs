using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.Admin
{
    public interface IMoneyRoleManager
    {
        Task<string> ChangeUserRole(string userName, string roleName);
        Task<bool> CreateRole(string roleName);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IEnumerable<UserRole>> GetRolesAsync();
    }
}
