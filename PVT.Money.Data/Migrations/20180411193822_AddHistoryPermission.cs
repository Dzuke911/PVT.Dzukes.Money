using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PVT.Money.Data.Migrations
{
    public partial class AddHistoryPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO dbo.Permissions VALUES ('ViewFullHistory')");

            AddPermissionToRole("Admin", "ViewFullHistory", migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM dbo.Permissions WHERE Permission = 'ViewFullHistory'");
        }

        private void AddPermissionToRole(string roleName, string permissionName, MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO dbo.PermissionsToRoles VALUES (" +
            $"(SELECT dbo.AspNetRoles.Id FROM dbo.AspNetRoles WHERE dbo.AspNetRoles.Name = '{roleName}'), " +
            $"(SELECT dbo.Permissions.Id FROM dbo.Permissions WHERE dbo.Permissions.Permission = '{permissionName}'))");
        }
    }
}
