using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PVT.Money.Data.Migrations
{
    public partial class AddBasicAspEntries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO dbo.AspNetRoles (Name,NormalizedName) VALUES ('Admin','ADMIN')");
            migrationBuilder.Sql("INSERT INTO dbo.AspNetRoles (Name,NormalizedName) VALUES ('User','USER')");

            migrationBuilder.Sql("INSERT INTO [dbo].[AspNetUsers] ([AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName]) VALUES (0, N'a47cdd9c-7fc8-4aba-9988-fe248dc4957f', N'Dzuke911@gmail.com', 0, 1, NULL, N'DZUKE911@GMAIL.COM', N'DZUKE911', N'AQAAAAEAACcQAAAAED2NlVyeXWFHxKfhi7vI18K9+nRkLBKHZeyk5B8kAqbWtZ7g4csmFWe+8VlSJ3OVew==', NULL, 0, N'37fa3e42-58b2-4cf9-a8e2-bea8400ffe3c', 0, N'Dzuke911')");

            AddRoleToUser("Dzuke911", "Admin", migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM dbo.AspNetUsers WHERE UserName = 'Dzuke911'");

            migrationBuilder.Sql("DELETE FROM dbo.AspNetRoles WHERE Name = 'Admin'");
            migrationBuilder.Sql("DELETE FROM dbo.AspNetRoles WHERE Name = 'User'");
        }

        private void AddRoleToUser(string userName, string roleName, MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO [dbo].[AspNetUserRoles] VALUES("+            
            $"(SELECT dbo.AspNetUsers.Id FROM dbo.AspNetUsers WHERE dbo.AspNetUsers.UserName = '{userName}'), " +
            $"(SELECT dbo.AspNetRoles.Id FROM dbo.AspNetRoles WHERE dbo.AspNetRoles.Name = '{roleName}'))");
        }
    }
}
