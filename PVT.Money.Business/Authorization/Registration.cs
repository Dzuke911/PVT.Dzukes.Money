using PVT.Money.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using PVT.Money.Business.Enums;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PVT.Money.Business.Authorization;

namespace PVT.Money.Business
{
    public class Registration :IRegistration
    {
        protected DatabaseContext DBContext { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        internal Registration(
                    UserManager<ApplicationUser> userManager,
                    SignInManager<ApplicationUser> signInManager,
                    DatabaseContext dbContext)
        {
            DBContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Checks for the existence of a user with the same login
        public async Task<bool> LoginAlreadyExistsAsync(string login)
        {
            //return await DBContext.OldUsers.AnyAsync(u => u.UserName == login);
            return (await _userManager.FindByNameAsync(login) != null);
        }

        // Checks for the existence of a user with the same email
        public async Task<bool> EmailAlreadyExistsAsync(string email)
        {
            //return await DBContext.OldUsers.AnyAsync(u => u.Email == email);
            return (await _userManager.FindByEmailAsync(email) != null);
        }

        // Register a new user
        //public async Task<RegistrationResult> Register(string login, string email, string password)
        //{
        //    if (await DBContext.Users.AnyAsync(u => u.UserName == login))
        //        return RegistrationResult.LoginAlreadyExists;
        //    if (await DBContext.Users.AnyAsync(u => u.Email == email))
        //        return RegistrationResult.LoginAlreadyExists;

        //    UserEntity user = new UserEntity
        //    {
        //        UserName = login,
        //        Email = email,
        //        Password = password,
        //        Role = await DBContext.Roles.FirstOrDefaultAsync(r => r.Role == "User"),
        //    };

        //    await DBContext.Users.AddAsync(user);
        //    await DBContext.SaveChangesAsync();

        //    await DBContext.UsersInfo.AddAsync(new UserInfoEntity
        //    {
        //        UserID = user.ID
        //    });

        //    await DBContext.SaveChangesAsync();
        //    return RegistrationResult.Success;
        //}

        public async Task<RegistrationResult> RegisterAsync(string login, string email)
        {
            if (await LoginAlreadyExistsAsync(login))
                return RegistrationResult.LoginAlreadyExists;
            if (await EmailAlreadyExistsAsync(email))
                return RegistrationResult.EmailAlreadyExists;

            ApplicationUser user = new ApplicationUser { UserName = login, Email = email };
            IdentityResult result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                await DBContext.UsersInfo.AddAsync(new UserInfoEntity { UserID = user.Id });
                await _userManager.AddToRoleAsync(user, "User");
                return RegistrationResult.Success;
            }
            else
                return RegistrationResult.Fail;
        }

        public async Task<RegistrationResult> RegisterAsync(string login, string email, string password)
        {
            if (await LoginAlreadyExistsAsync(login))
                return RegistrationResult.LoginAlreadyExists;
            if (await EmailAlreadyExistsAsync(email))
                return RegistrationResult.EmailAlreadyExists;

            ApplicationUser user = new ApplicationUser { UserName = login, Email = email };
            IdentityResult result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await DBContext.UsersInfo.AddAsync(new UserInfoEntity {UserID = user.Id} );
                await _userManager.AddToRoleAsync(user, "User");
                return RegistrationResult.Success;
            }                
            else
                return RegistrationResult.Fail;
            //{
            //    //              await LogWriterProp.Write(model.Login, $"New user '{model.Login}' was registered.");

            //    var code = await userManager.GenerateEmailConfirmationTokenAsync((WebApplicationUser)user);
            //    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
            //    //await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

            //    await _signInManager.SignInAsync(user, isPersistent: false);
            //    return View("Login");
            //}
            //AddErrors(result);
        }
    }
}
