using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PVT.Money.Business.Enums;
using PVT.Money.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.Admin
{
    public class MoneyRoleManager : IMoneyRoleManager
    {
        protected DatabaseContext DBContext { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        internal MoneyRoleManager(
            DatabaseContext dbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            DBContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> ChangeUserRole(string userName, string roleName)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                throw new InvalidOperationException($"There is no {userName} user in database");
            else
            {
                if(!await _roleManager.RoleExistsAsync(roleName))
                    throw new InvalidOperationException($"There is no {roleName} role in database");
                else
                {
                    IEnumerable<string> rolesNames = await _roleManager.Roles.Select(r => r.Name).ToArrayAsync();
                    await _userManager.RemoveFromRolesAsync(user,rolesNames);
                    await _userManager.AddToRoleAsync(user, roleName);

                    return $"{user.UserName}  role changed to {roleName}";
                }
            }
        }

        public async Task<bool> CreateRole(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                return false;
            else
            {
                await _roleManager.CreateAsync(new ApplicationRole() { Name = roleName });
                return true;
            }
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<IEnumerable<UserRole>> GetRolesAsync()
        {
            List<UserRole> result = new List<UserRole>();

            IEnumerable<ApplicationRole> applicationRoles = await _roleManager.Roles.ToListAsync();

            foreach (ApplicationRole r in applicationRoles)
            {
                await DBContext.Entry(r).Collection(ar => ar.RolePermissions).LoadAsync();
                foreach (PermissionToRoleEntity ptr in r.RolePermissions)
                {
                    DBContext.Entry(ptr).Reference(p => p.Permission).Load();
                }

                UserRole newRole = new UserRole();
                newRole.Name = r.Name;
                newRole.Permissions = "";
                foreach (PermissionToRoleEntity ptr in r.RolePermissions)
                {
                    newRole.Permissions += ptr.Permission.Permission + ',';
                }
                newRole.Permissions = newRole.Permissions.TrimEnd(',');
                result.Add(newRole);
            }

            result.Sort((r1, r2) => r1.Name.CompareTo(r2.Name));
            return result;
        }
    }
}
