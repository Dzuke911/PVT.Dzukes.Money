using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PVT.Money.Business.Admin;
using PVT.Money.Business.Authorization;
using PVT.Money.Business.Logger;
using PVT.Money.Business.Main;
using PVT.Money.Business.Management;
using PVT.Money.Business.MoneyClasses.ForGlobalRates;
using PVT.Money.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business.Facade
{
    public static class BuisenessFacade
    {        
        public static void ConfigureServices(IServiceCollection services)
        {
            DatabaseFacade.Configuration = Configuration;
            DatabaseFacade.ConfigureServices(services);
            DatabaseFacade.InitDatabase();

            services.AddTransient<IAuthentication, Authentication>(f=>new Authentication(f.GetService<SignInManager<ApplicationUser>>(), f.GetService<UserManager<ApplicationUser>>()));
            services.AddTransient<IRegistration, Registration>(f => new Registration(f.GetService<UserManager<ApplicationUser>>(), f.GetService<SignInManager<ApplicationUser>>(), f.GetService<DatabaseContext>()));
            services.AddTransient<IMoneyRoleManager, MoneyRoleManager>(f => new MoneyRoleManager(f.GetService<DatabaseContext>(), f.GetService<UserManager<ApplicationUser>>(), f.GetService<RoleManager<ApplicationRole>>()));
            services.AddTransient<IMoneyUserManager, MoneyUserManager>(f => new MoneyUserManager(f.GetService<DatabaseContext>(), f.GetService<UserManager<ApplicationUser>>()));
            services.AddTransient<ILogManager, LogManager>(f => new LogManager(f.GetService<DatabaseContext>(), f.GetService<UserManager<ApplicationUser>>()));
            services.AddTransient<IUserInfoManager, UserInfoManager>(f => new UserInfoManager(f.GetService<DatabaseContext>(), f.GetService<UserManager<ApplicationUser>>()));
            services.AddTransient<IPermissionManager, PermissionManager>(f => new PermissionManager(f.GetService<DatabaseContext>(), f.GetService<RoleManager<ApplicationRole>>()));
            services.AddTransient<IAccountDatabaseManager,AccountDatabaseManager>(f => new AccountDatabaseManager(f.GetService<DatabaseContext>(), f.GetService<UserManager<ApplicationUser>>(), f.GetService<IGlobalRates>()));
            services.AddTransient<IMoneyImageParser, MoneyImageParser>(f => new MoneyImageParser(f.GetService<DatabaseContext>(), f.GetService<UserManager<ApplicationUser>>()));
            services.AddTransient<IGlobalRates, GlobalRates>(f => new GlobalRates());
            services.AddTransient<IEmailSender, EmailSender>(f => new EmailSender());
        }

        public static IConfiguration Configuration { get; set; }
    }
}
