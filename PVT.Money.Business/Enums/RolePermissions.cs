using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business.Enums
{
    public enum RolePermissions
    {
        CreateAccount,
        PutToAccount,
        WithdrawFromAccount,
        Transact,
        Convert,
        ComissionSubscribe,
        DeleteAccount,
        ManageUsers,
        ManageRoles,
        ViewFullHistory
    }
}
