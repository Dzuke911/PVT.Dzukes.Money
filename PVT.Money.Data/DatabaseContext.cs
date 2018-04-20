using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PVT.Money.Data
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<LogEventEntity> LogEvents { get; set; }
        public DbSet<UserInfoEntity> UsersInfo { get; set; }
        public DbSet<AccountEntity> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PermissionToRoleEntity>().HasKey(p => new { p.RoleID, p.PermissionID });
            modelBuilder.Entity<PermissionEntity>().HasIndex(p => p.Permission).IsUnique();
        }
    }
}
