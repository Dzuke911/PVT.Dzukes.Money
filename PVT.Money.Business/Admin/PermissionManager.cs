using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PVT.Money.Business.Enums;
using PVT.Money.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.Main
{
    public class PermissionManager : IPermissionManager
    {
        protected DatabaseContext DBContext { get; set; }
        private readonly RoleManager<ApplicationRole> _roleManager;

        internal PermissionManager(
            DatabaseContext dbContext,
            RoleManager<ApplicationRole> roleManager)
        {
            DBContext = dbContext;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Adds permission to role
        /// </summary>
        public async Task AddPermissionToRoleAsync(string roleName, RolePermissions permission)
        {
            await AddPermissionToRoleAsync(roleName, permission.ToString());
        }

        /// <summary>
        /// Adds permission to role
        /// </summary>
        public async Task AddPermissionToRoleAsync(string roleName, string permissionName)
        {
            ApplicationRole role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                throw new InvalidOperationException($"There is no '{roleName}' role in database.");
            await DBContext.Entry(role).Collection(r => r.RolePermissions).LoadAsync();

            PermissionEntity permission = await DBContext.Permissions.FirstOrDefaultAsync(p => p.Permission == permissionName);
            if (permission == null)
                throw new InvalidOperationException($"There is no '{permissionName}' permission in database.");

            if (role.RolePermissions == null)
                role.RolePermissions = new List<PermissionToRoleEntity>();
            else
            {
                foreach (PermissionToRoleEntity ptr in role.RolePermissions)
                {
                    if (ptr.Permission.Permission == permissionName)
                        throw new InvalidOperationException($"'{roleName}' already has '{permissionName}' permission.");
                }
            }

            role.RolePermissions.Add(new PermissionToRoleEntity() { Permission = permission });

            await _roleManager.UpdateAsync(role);
        }

        public async Task RemovePermissionFromRoleAsync(string roleName, RolePermissions permission)
        {
            await RemovePermissionFromRoleAsync(roleName, permission.ToString());
        }

        public async Task RemovePermissionFromRoleAsync(string roleName, string permissionName)
        {
            ApplicationRole role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                throw new InvalidOperationException($"There is no '{roleName}' role in database");
            await DBContext.Entry(role).Collection(r => r.RolePermissions).LoadAsync();

            PermissionEntity permission = await DBContext.Permissions.Include(p => p.PermissionRoles).FirstOrDefaultAsync(p => p.Permission == permissionName);
            if (permission == null)
                throw new InvalidOperationException($"There is no '{permissionName}' permission in database");

            if (role.RolePermissions == null)
                throw new InvalidOperationException($"'{roleName}' role already hasn`t '{permissionName}' permission");

            if (!role.RolePermissions.Any(ptr => ptr.Permission.Permission == permissionName))
                throw new InvalidOperationException($"'{roleName}' role already hasn`t '{permissionName}' permission");

            PermissionToRoleEntity ptrToRemove = role.RolePermissions.FirstOrDefault(ptr => ptr.Permission.Permission == permissionName);

            role.RolePermissions.Remove(ptrToRemove);

            await _roleManager.UpdateAsync(role);
        }

        public async Task<IEnumerable<string>> GetPermissionsAsStringsAsync()
        {
            return await DBContext.Permissions.Select(p => p.Permission).ToArrayAsync();
        }

        /// <summary>
        /// Returns role`s permissions as enumeration of strings. 
        /// </summary>
        /// <exception cref = "InvalidOperationException">Thrown when 'roleName' doesn`t exist in database.</exception>
        public async Task<IEnumerable<string>> GetRolePermissionsAsStringsAsync(string roleName)
        {
            ApplicationRole role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
                throw new InvalidOperationException($"There is no '{roleName}' role in database");

            await DBContext.Entry(role).Collection(r => r.RolePermissions).LoadAsync();

            foreach (var rp in role.RolePermissions)
            {
                await DBContext.Entry(rp).Reference(i => i.Permission).LoadAsync();
            }

            return role.RolePermissions.Select(rp => rp.Permission.Permission);
        }

        /// <summary>
        /// Checks for the existence of permissions in the database. Throws InvalidOperationException if one of them not exists.
        /// </summary>
        public async Task IsPermissionsExistsAsync(params string[] permissionsNames)
        {
            IEnumerable<PermissionEntity> perms = await DBContext.Permissions.ToArrayAsync();

            foreach (string name in permissionsNames)
            {
                if (!perms.Any(p => p.Permission == name))
                    throw new InvalidOperationException($"There is no '{name}' permission in database");
            }
        }

        public async Task<IEnumerable<string>> GetPermissionsToAddAsync(string roleName, IEnumerable<string> newPermissions)
        {
            List<string> result = new List<string>();
            IEnumerable<string> currentPermissions = await GetRolePermissionsAsStringsAsync(roleName);

            try{ await IsPermissionsExistsAsync(newPermissions.ToArray()); }
            catch(InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }

            foreach (string p in newPermissions)
            {
                if (!currentPermissions.Any(cp => cp == p))
                    result.Add(p);
            }
            return result;
        }

        public async Task<IEnumerable<string>> GetPermissionsToRemoveAsync(string roleName, IEnumerable<string> newPermissions)
        {
            List<string> result = new List<string>();
            IEnumerable<string> currentPermissions = await GetRolePermissionsAsStringsAsync(roleName);

            try { await IsPermissionsExistsAsync(newPermissions.ToArray()); }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }

            foreach (string p in currentPermissions)
            {
                if (!newPermissions.Any(np => np == p))
                    result.Add(p);
            }
            return result;
        }

        public bool IsRoleHavePermission(string roleName, RolePermissions permission)
        {
            Task<ApplicationRole> task = _roleManager.FindByNameAsync(roleName);
            ApplicationRole role = task.Result;

            if (role == null)
                throw new InvalidOperationException($"There is no '{roleName}' role in database");

            DBContext.Entry(role).Collection(r => r.RolePermissions).Load();

            List<PermissionEntity> permissions = new List<PermissionEntity>();

            foreach (var rp in role.RolePermissions)
            {
                DBContext.Entry(rp).Reference(i => i.Permission).Load();
                permissions.Add(rp.Permission);
            }

            return permissions.Any(p => p.Permission == permission.ToString());
        }

        public async Task<IEnumerable<Claim>> GetRolePermissionsAsClaimsAsync(string roleName)
        {
            ApplicationRole role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
                throw new InvalidOperationException($"There is no '{roleName}' role in database");

            await DBContext.Entry(role).Collection(r => r.RolePermissions).LoadAsync();
            List<Claim> claims = new List<Claim>();

            foreach (var rp in role.RolePermissions)
            {
                await DBContext.Entry(rp).Reference(i => i.Permission).LoadAsync();
                claims.Add(new Claim("PermissionClaim", rp.Permission.Permission));
            }

            return claims ;
        }
    }
}
