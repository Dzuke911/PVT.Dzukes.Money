using Microsoft.EntityFrameworkCore;
using PVT.Money.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using PVT.Money.Business.Authorization;
using PVT.Money.Business.Admin;
using PVT.Money.Business.Main;
using PVT.Money.Business.Logger;
using System.Data.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace PVT.Money.Business.Tests.FakeClasses
{
    public class BusinessTestMethods 
    {
        const string testDatabaseName = "TestDatabase";
        protected readonly IServiceCollection _serviceCollection;
        protected readonly DatabaseContext _dbContext;
        protected readonly RoleManager<ApplicationRole> _roleManager;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly SignInManager<ApplicationUser> _signInManager;
        
        public BusinessTestMethods()
        {
            _serviceCollection = new ServiceCollection();

            _serviceCollection.AddDbContext<DatabaseContext>(options => options.UseInMemoryDatabase(testDatabaseName), ServiceLifetime.Singleton);

            _serviceCollection.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
            })
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            _dbContext = _serviceCollection.BuildServiceProvider().GetRequiredService<DatabaseContext>();

            RoleManager<ApplicationRole> tempRoleManager = _serviceCollection.BuildServiceProvider().GetRequiredService<RoleManager<ApplicationRole>>();
            UserManager<ApplicationUser> tempUserManager = _serviceCollection.BuildServiceProvider().GetRequiredService<UserManager<ApplicationUser>>();
            SignInManager<ApplicationUser> tempSignInManager = _serviceCollection.BuildServiceProvider().GetRequiredService<SignInManager<ApplicationUser>>();

            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser,ApplicationRole, DatabaseContext, int>(_dbContext),
                _serviceCollection.BuildServiceProvider().GetRequiredService<IOptions<IdentityOptions>>(),
                tempUserManager.PasswordHasher,
                tempUserManager.UserValidators,
                tempUserManager.PasswordValidators,
                tempUserManager.KeyNormalizer,
                tempUserManager.ErrorDescriber,
                null,
                _serviceCollection.BuildServiceProvider().GetRequiredService<ILogger<UserManager<ApplicationUser>>>()
                );

            _roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole, DatabaseContext, int>(_dbContext), 
                tempRoleManager.RoleValidators, 
                tempRoleManager.KeyNormalizer, 
                tempRoleManager.ErrorDescriber,
                _serviceCollection.BuildServiceProvider().GetRequiredService<ILogger<RoleManager<ApplicationRole>>>()
                );

            _signInManager = new SignInManager<ApplicationUser>(_userManager,
                _serviceCollection.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>(),
                tempSignInManager.ClaimsFactory,
                null,
                _serviceCollection.BuildServiceProvider().GetRequiredService<ILogger<SignInManager<ApplicationUser>>>(),
                null
                );
        }
    }
}
