using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PVT.Money.Data.Migrations
{
    public partial class AddBasicEntries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add Permissions
            migrationBuilder.Sql("INSERT INTO dbo.Permissions VALUES ('ComissionSubscribe')");
            migrationBuilder.Sql("INSERT INTO dbo.Permissions VALUES ('Convert')");
            migrationBuilder.Sql("INSERT INTO dbo.Permissions VALUES ('CreateAccount')");
            migrationBuilder.Sql("INSERT INTO dbo.Permissions VALUES ('DeleteAccount')");
            migrationBuilder.Sql("INSERT INTO dbo.Permissions VALUES ('PutToAccount')");
            migrationBuilder.Sql("INSERT INTO dbo.Permissions VALUES ('Transact')");
            migrationBuilder.Sql("INSERT INTO dbo.Permissions VALUES ('WithdrawFromAccount')");

            // Add Roles
            migrationBuilder.Sql("INSERT INTO dbo.UserRoles VALUES ('Admin')");
            migrationBuilder.Sql("INSERT INTO dbo.UserRoles VALUES ('User')");

            // Add BasicAdmin
            migrationBuilder.Sql("INSERT INTO dbo.Users VALUES (" +
                "'Dzuke911@gmail.com', " +
                "'WaRDuke911atl', " +
                "(SELECT dbo.UserRoles.Id FROM dbo.UserRoles WHERE dbo.UserRoles.Role = 'Admin'), " +
                "'Dzuke911')");

            // Add Permissions to Roles
            AddPermissionToRole("Admin", "ComissionSubscribe", migrationBuilder);
            AddPermissionToRole("Admin", "Convert", migrationBuilder);
            AddPermissionToRole("Admin", "CreateAccount", migrationBuilder);
            AddPermissionToRole("Admin", "DeleteAccount", migrationBuilder);
            AddPermissionToRole("Admin", "PutToAccount", migrationBuilder);
            AddPermissionToRole("Admin", "Transact", migrationBuilder);
            AddPermissionToRole("Admin", "WithdrawFromAccount", migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //delete basic admin user
            migrationBuilder.Sql("DELETE FROM dbo.Users WHERE UserName = 'Dzuke911'");

            //delete basic roles
            migrationBuilder.Sql("DELETE FROM dbo.UserRoles WHERE Role = 'Admin'");
            migrationBuilder.Sql("DELETE FROM dbo.UserRoles WHERE Role = 'User'");

            //delete basic permissions
            migrationBuilder.Sql("DELETE FROM dbo.Permissions WHERE Permission = 'ComissionSubscribe'");
            migrationBuilder.Sql("DELETE FROM dbo.Permissions WHERE Permission = 'Convert'");
            migrationBuilder.Sql("DELETE FROM dbo.Permissions WHERE Permission = 'CreateAccount'");
            migrationBuilder.Sql("DELETE FROM dbo.Permissions WHERE Permission = 'DeleteAccount'");
            migrationBuilder.Sql("DELETE FROM dbo.Permissions WHERE Permission = 'PutToAccount'");
            migrationBuilder.Sql("DELETE FROM dbo.Permissions WHERE Permission = 'Transact'");
            migrationBuilder.Sql("DELETE FROM dbo.Permissions WHERE Permission = 'WithdrawFromAccount'");
        }

        private void AddPermissionToRole(string roleName, string permissionName, MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO dbo.PermissionsToRoles VALUES (" +
            $"(SELECT dbo.UserRoles.Id FROM dbo.UserRoles WHERE dbo.UserRoles.Role = '{roleName}'), " +
            $"(SELECT dbo.Permissions.Id FROM dbo.Permissions WHERE dbo.Permissions.Permission = '{permissionName}'))");
        }
    }
}
