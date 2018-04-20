using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PVT.Money.Shell.Web.Models;
using PVT.Money.Business;
using PVT.Money.Business.Enums;
using PVT.Money.Shell.Web.Binders;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Reflection;
using PVT.Money.Shell.Web.Container;
using PVT.Money.Business.Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PVT.Money.Business.Authorization;
using PVT.Money.Business.Main;
using PVT.Money.Shell.Web.Attributes;
using Microsoft.AspNetCore.SignalR;
using PVT.Money.Shell.Web.Hubs;
using System.Threading;
using PVT.Money.Shell.Web.Identity;

namespace PVT.Money.Shell.Web.Controllers
{
    ///////////////////////////////////////
    /// ANTIPATTERN: ANCHOR, HARD CODE
    ///////////////////////////////////////
    [Authorize]
    public class AccountController : Controller
    {
        public IMoneyContainer Container { get; }
        private readonly IRegistration _registration;
        private readonly IAuthentication _authentication;
        private readonly ILogManager _logManager;
        private readonly IEmailSender _emailSender;
        private readonly IMoneyUserManager _moneyUserManager;
        private readonly IPermissionManager _permissionManager;
        private readonly IHubContext<MoneyHub> _testHubContext;

        public AccountController(IMoneyContainer container,
            IRegistration registration,
            IAuthentication authentication,
            ILogManager logManager,
            IEmailSender emailSender,
            IMoneyUserManager moneyUserManager,
            IPermissionManager permissionManager,
            IHubContext<MoneyHub> testHubContext)
        {
            Container = container;
            _registration = registration;
            _authentication = authentication;
            _logManager = logManager;
            _emailSender = emailSender;
            _moneyUserManager = moneyUserManager;
            _testHubContext = testHubContext;
            _permissionManager = permissionManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrationFormAction(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                RegistrationResult result = await _registration.RegisterAsync(model.Login, model.Email, model.Password);
                if (result == RegistrationResult.Success)
                {
                    await _logManager.WriteAsync(model.Login, $"New user '{model.Login}' was registered.");
                    return View("Login");
                }
                else
                    return View();
            }

            return View("Registration");

            //////////////////////////////
            ///// OLD FORMS REGISTRATION
            //////////////////////////////
            /*   if (model.Password != model.ConfirmPassword)
               {
                   ViewData["Error"] = "Password does not match the confirm password";
                   return View("Registration", model);
               }

               RegistrationResult result = await RegistrationProp.Register(model.Login, model.Email, model.Password);

               switch (result)
               {
                   case RegistrationResult.LoginAlreadyExists:
                       {
                           ViewData["Error"] = "This login is already in use on Money";
                           return View("Registration", model);
                       }
                   case RegistrationResult.EmailAlreadyExists:
                       {
                           ViewData["Error"] = "This email address is already in use on Money";
                           return View("Registration", model);
                       }
                   case RegistrationResult.Success:
                       {
                           await LogWriterProp.Write(model.Login, $"New user '{model.Login}' was registered.");
                           return View("Login");
                       }
                   default:
                       {
                           ViewData["Error"] = "Unknown error";
                           return View("Registration", model);
                       }
               }*/
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Registration()
        {
            return await Task.FromResult(View());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> LoginExists(string login)
        {
            return Json(!await _registration.LoginAlreadyExistsAsync(login));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> EmailExists(string email)
        {
            return Json(!await _registration.EmailAlreadyExistsAsync(email));
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignInFormAction([ModelBinder(BinderType = typeof(SignInBinder))]SignInModel model)
        {
            bool lockoutOnFailure = false;

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                Microsoft.AspNetCore.Identity.SignInResult result = await _authentication.SignInAsync(model.Login, model.Password, model.RememberMe, lockoutOnFailure);
                if (result.Succeeded)
                {
                    string userName = await _moneyUserManager.GetUserNameAsync(model.Login);
                    await _logManager.WriteAsync(userName, $"User '{userName}' signed in.");

                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                //if (result.RequiresTwoFactor)
                //{
                //    return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
                //}
                if (result.IsLockedOut)
                {
                    await _logManager.WriteAsync(model.Login, $"User '{model.Login}' tried to sign in but it`s locked");
                    return RedirectToAction(nameof(AccountController.Login));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    ViewData["WrongSignIn"] = "Wrong combination of login and/or password";
                    return View(nameof(AccountController.Login));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(nameof(AccountController.Login));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            //Task.Run(() =>
            //{
            //    Thread.Sleep(10000);
            //    SendTestSignalRMessage();
            //});

            return await Task.FromResult(View());
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> About()
        {
            ViewData["Message"] = "Your application description page.";

            return await Task.FromResult(View());
        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            string name = User.Identity.Name;

            await _authentication.SignOutAsync();

            await _logManager.WriteAsync(name, $"User '{name}' signed out.");

            return RedirectToAction(nameof(AccountController.Login));
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PasswordRecoveryFormAction(PasswordRecoveryModel model)
        {
            int id;
            string code = await _moneyUserManager.GeneratePasswordResetTokenAsync(model.Email);
            if (code == null)
                return View(nameof(AccountController.Login));

            try
            {
                id = await _moneyUserManager.GetUserIdAsync(model.Email);
            }
            catch
            {
                return View(nameof(AccountController.Login));
            }

            string callbackUrl = Url.ResetPasswordCallbackLink(id, code, Request.Scheme);

            await _emailSender.SendEmailAsync(model.Email, "Password recovery", $"Please reset your password on PVT.Money by clicking here: <a href='{callbackUrl}'>link</a>");

            string userName = await _moneyUserManager.GetUserNameAsync(model.Email);
            if (userName != null)
                await _logManager.WriteAsync(userName, $"User '{userName}' requested password recovery.");

            return View("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PasswordRecovery(PasswordRecoveryModel model)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            string redirectUrl;
            // Request a redirect to the external login provider.
            redirectUrl = $"Account/{nameof(ExternalLoginCallback)}/";//Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            AuthenticationProperties properties = _authentication.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                //ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(AccountController.Login));
            }
            ExternalLoginInfo info = await _authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(AccountController.Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            bool isPersistent = false;
            bool bypassTwoFactor = true;

            Microsoft.AspNetCore.Identity.SignInResult result = await _authentication.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent, bypassTwoFactor);
            if (result.Succeeded)
            {
                //_logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                //return RedirectToLocal(returnUrl);
                string userName = await _moneyUserManager.GetUserNameAsync(info.LoginProvider, info.ProviderKey);
                await _logManager.WriteAsync(userName, $"User '{userName}' signed in with {info.ProviderDisplayName}.");
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(AccountController.Login)); // RedirectToAction(nameof(Lockout));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                //ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View(nameof(AccountController.ExternalLogin), new ExternalLoginModel { Login = null, Email = email });
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginFormAction(ExternalLoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                ExternalLoginInfo info = await _authentication.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                //var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                //var result = await _userManager.CreateAsync(user);
                RegistrationResult result = await _registration.RegisterAsync(model.Login, model.Email);
                if (result == RegistrationResult.Success)
                {

                    IdentityResult identResult = await _moneyUserManager.AddLoginAsync(model.Login, info);
                    if (identResult.Succeeded)
                    {
                        //await _signInManager.SignInAsync(user, isPersistent: false);
                        await _authentication.SignInAsync(model.Login, false);
                        await _logManager.WriteAsync(model.Login, $"New user '{model.Login}' was registered with {info.ProviderDisplayName}.");
                        //_logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        //return RedirectToLocal(returnUrl);
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }
                //AddErrors(result);
            }
            return View(nameof(ExternalLogin), model);
        }

        private void SendTestSignalRMessage()
        {
            // _testHubContext.Clients.All.InvokeAsync("Send","HelloSignalR");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new PasswordCreationModel { Code = code };
            return View(nameof(AccountController.PasswordCreation), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PasswordCreation()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePasswordFormAction(PasswordCreationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(AccountController.Login));
            }

            string userName = await _moneyUserManager.CreatePasswordAsync(model.Login, model.Password, model.Code);

            if (userName != null)
                await _logManager.WriteAsync(userName, $"User '{userName}' recovered his password");

            return View(nameof(AccountController.Login));
        }
    }
}
