using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PVT.Money.Data.Migrations
{
    public partial class AddAdminPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO dbo.Permissions VALUES ('ManageUsers')");
            migrationBuilder.Sql("INSERT INTO dbo.Permissions VALUES ('ManageRoles')");

            AddPermissionToRole("Admin", "ManageUsers", migrationBuilder);
            AddPermissionToRole("Admin", "ManageRoles", migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM dbo.Permissions WHERE Permission = 'ManageUsers'");
            migrationBuilder.Sql("DELETE FROM dbo.Permissions WHERE Permission = 'ManageRoles'");
        }

        private void AddPermissionToRole(string roleName, string permissionName, MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO dbo.PermissionsToRoles VALUES (" +
            $"(SELECT dbo.AspNetRoles.Id FROM dbo.AspNetRoles WHERE dbo.AspNetRoles.Name = '{roleName}'), " +
            $"(SELECT dbo.Permissions.Id FROM dbo.Permissions WHERE dbo.Permissions.Permission = '{permissionName}'))");
        }
    }
}
