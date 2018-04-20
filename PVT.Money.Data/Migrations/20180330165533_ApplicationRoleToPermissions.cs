using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PVT.Money.Data.Migrations
{
    public partial class ApplicationRoleToPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM dbo.PermissionsToRoles");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionsToRoles_AspNetRoles_RoleId",
                table: "PermissionsToRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionsToRoles_AspNetRoles_RoleId",
                table: "PermissionsToRoles");

            // Add Permissions to Roles
            AddPermissionToRole("Admin", "ComissionSubscribe", migrationBuilder);
            AddPermissionToRole("Admin", "Convert", migrationBuilder);
            AddPermissionToRole("Admin", "CreateAccount", migrationBuilder);
            AddPermissionToRole("Admin", "DeleteAccount", migrationBuilder);
            AddPermissionToRole("Admin", "PutToAccount", migrationBuilder);
            AddPermissionToRole("Admin", "Transact", migrationBuilder);
            AddPermissionToRole("Admin", "WithdrawFromAccount", migrationBuilder);
        }

        private void AddPermissionToRole(string roleName, string permissionName, MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO dbo.PermissionsToRoles VALUES (" +
            $"(SELECT dbo.UserRoles.Id FROM dbo.UserRoles WHERE dbo.UserRoles.Role = '{roleName}'), " +
            $"(SELECT dbo.Permissions.Id FROM dbo.Permissions WHERE dbo.Permissions.Permission = '{permissionName}'))");
        }
    }
}
