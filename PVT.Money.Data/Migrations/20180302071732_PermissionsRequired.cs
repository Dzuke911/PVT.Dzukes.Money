using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PVT.Money.Data.Migrations
{
    public partial class PermissionsRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Permissions_Permission",
                table: "Permissions");

            migrationBuilder.AlterColumn<string>(
                name: "Permission",
                table: "Permissions",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Permission",
                table: "Permissions",
                column: "Permission",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Permissions_Permission",
                table: "Permissions");

            migrationBuilder.AlterColumn<string>(
                name: "Permission",
                table: "Permissions",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Permission",
                table: "Permissions",
                column: "Permission",
                unique: true,
                filter: "[Permission] IS NOT NULL");
        }
    }
}
