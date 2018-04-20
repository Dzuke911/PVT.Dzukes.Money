using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PVT.Money.Data.Migrations
{
    public partial class DropOldTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Users_UserId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_LogEvents_Users_UserId",
                table: "LogEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_PermissionsToRoles_UserRoles_RoleId",
                table: "PermissionsToRoles");

            migrationBuilder.DropTable("Users");

            migrationBuilder.DropTable("UserRoles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
