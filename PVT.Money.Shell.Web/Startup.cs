using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using PVT.Money.Shell.Web.Container;
using PVT.Money.Business.Logger;
using PVT.Money.Business;
using PVT.Money.Business.Admin;
using PVT.Money.Business.Main;
using PVT.Money.Business.Facade;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PVT.Money.Shell.Web.Hubs;
using PVT.Money.Shell.Web.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace PVT.Money.Shell.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            // Add application services.

            services.AddAuthentication().AddGoogle(options => { options.ClientId = "34038796331-in4repsdgkmea6t145os8bm5eaua79fc.apps.googleusercontent.com"; options.ClientSecret = "PggquUC9na3IMnTQOfktIZto"; });
            services.AddAuthentication().AddFacebook(options => { options.AppId = "1170050669797001"; options.AppSecret = "0bc2f0428dc209fcf0582c93f8ef3e0e"; });
            services.AddAuthentication().AddTwitter(options => { options.ConsumerKey = "iAZzuoluuKt6IwqCgyReWjq8D"; options.ConsumerSecret = "wFINIn3CKMeWzhNzRo8gfHv2iZo2LDWR4SB2A9vMzFePkJlZnj"; });
            services.AddSignalR();
            services.AddTransient<IClaimsTransformation, MoneyClaimsTransformation>(f => new MoneyClaimsTransformation(f.GetService<IMoneyUserManager>(), f.GetService<IPermissionManager>()));

            MoneyContainer container = new MoneyContainer();
            container.Add<ClassA>();
            container.Add<ClassB>();

            BuisenessFacade.Configuration = Configuration;
            BuisenessFacade.ConfigureServices(services);
            services.AddSingleton<IMoneyContainer, MoneyContainer>(i => container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSignalR(routes =>
            {
                    routes.MapHub<MoneyHub>("MoneyHub");
            });
        }
    }
}
