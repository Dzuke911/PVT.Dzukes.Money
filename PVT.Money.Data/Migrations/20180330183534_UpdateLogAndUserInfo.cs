using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PVT.Money.Data.Migrations
{
    public partial class UpdateLogAndUserInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM dbo.UsersInfo");
            migrationBuilder.Sql("DELETE FROM dbo.LogEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersInfo_Users_UserId",
                table: "UsersInfo");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_Role",
                table: "UserRoles");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "UserRoles",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_LogEvents_AspNetUsers_UserId",
                table: "LogEvents",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInfo_AspNetUsers_UserId",
                table: "UsersInfo",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.Sql("INSERT INTO [dbo].[UsersInfo] ( [Address], [BirthDate], [FirstName], [Gender], [LastName], [Phone], [UserId]) VALUES ( NULL, NULL, NULL, NULL, NULL, NULL, (SELECT dbo.AspNetUsers.Id FROM dbo.AspNetUsers WHERE dbo.AspNetUsers.UserName = 'Dzuke911'))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM dbo.UsersInfo WHERE UserId = (SELECT dbo.AspNetUsers.Id FROM dbo.AspNetUsers WHERE dbo.AspNetUsers.UserName = 'Dzuke911')");

            migrationBuilder.DropForeignKey(
                name: "FK_LogEvents_AspNetUsers_UserId",
                table: "LogEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersInfo_AspNetUsers_UserId",
                table: "UsersInfo");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "UserRoles",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_Role",
                table: "UserRoles",
                column: "Role",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInfo_Users_UserId",
                table: "UsersInfo",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
