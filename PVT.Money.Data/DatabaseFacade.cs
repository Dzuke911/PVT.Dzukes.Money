using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Data
{
    public static class DatabaseFacade
    {
        public static IConfiguration Configuration { get; set; }

        public static void InitDatabase(string connectionString = null)
        {
            if (connectionString == null)
                connectionString = Configuration.GetConnectionString("Database");

            DbContextOptionsBuilder<DatabaseContext> contextOptionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            contextOptionsBuilder.UseSqlServer(connectionString);
            DbContextOptions<DatabaseContext> options = contextOptionsBuilder.Options;
            using (var context = new DatabaseContext(options))
            {
                context.Database.Migrate();
            }
        }

        public static void ConfigureServices(IServiceCollection services)
        {          
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Database")));

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
            })
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();
        }
    }
}
