using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business.Enums
{
    public static class RolePermissionsExtensions
    {
        public static RolePermissions ToRolePermissions(this string permissionName)
        {
            switch (permissionName)
            {
                case "ComissionSubscribe":
                    return RolePermissions.ComissionSubscribe;
                case "Convert":
                    return RolePermissions.Convert;
                case "CreateAccount":
                    return RolePermissions.CreateAccount;
                case "DeleteAccount":
                    return RolePermissions.DeleteAccount;
                case "PutToAccount":
                    return RolePermissions.PutToAccount;
                case "Transact":
                    return RolePermissions.Transact;
                case "WithdrawFromAccount":
                    return RolePermissions.WithdrawFromAccount;
                case "ManageRoles":
                    return RolePermissions.ManageRoles;
                case "ManageUsers":
                    return RolePermissions.ManageUsers;
                case "ViewFullHistory":
                    return RolePermissions.ViewFullHistory;
                default:
                    throw new NotImplementedException("Can`t convert this string to permission");
            }
        }
    }
}
