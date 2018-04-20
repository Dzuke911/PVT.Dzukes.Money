using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PVT.Money.Data.Migrations
{
    public partial class AddCommissionAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO[dbo].[Accounts] ( [Amount], [Currency], [IsCommission], [UserId], [AccountName]) VALUES( CAST(0.0 AS Decimal(18, 2)), N'USD', 1, (SELECT dbo.AspNetUsers.Id FROM dbo.AspNetUsers WHERE dbo.AspNetUsers.UserName = 'Dzuke911'), 'Commission_account')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM[dbo].[Accounts] WHERE (UserId = (SELECT dbo.AspNetUsers.Id FROM dbo.AspNetUsers WHERE dbo.AspNetUsers.UserName = 'Dzuke911')) AND ([AccountName] = 'Commission_account')");
        }
    }
}
