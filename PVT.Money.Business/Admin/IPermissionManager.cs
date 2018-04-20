using PVT.Money.Business.Enums;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.Main
{
    public interface IPermissionManager
    {
        Task AddPermissionToRoleAsync(string roleName, RolePermissions permission);
        Task AddPermissionToRoleAsync(string roleName, string permissionName);
        Task RemovePermissionFromRoleAsync(string roleName, RolePermissions permission);
        Task RemovePermissionFromRoleAsync(string roleName, string permissionName);
        Task<IEnumerable<string>> GetPermissionsToAddAsync(string roleName, IEnumerable<string> newPermissions);
        Task<IEnumerable<string>> GetPermissionsToRemoveAsync(string roleName, IEnumerable<string> newPermissions);
        Task<IEnumerable<string>> GetPermissionsAsStringsAsync();
        Task<IEnumerable<Claim>> GetRolePermissionsAsClaimsAsync(string roleName);

        bool IsRoleHavePermission(string roleName, RolePermissions permission);
    }
}
