using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PVT.Money.Business;
using PVT.Money.Business.Admin;
using PVT.Money.Business.Main;
using PVT.Money.Shell.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Reflection;
using PVT.Money.Shell.Web.Attributes;
using PVT.Money.Shell.Web.Container;
using PVT.Money.Business.Logger;
using System.Security.Claims;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using PVT.Money.Business.Authorization;
using PVT.Money.Business.Enums;
using PVT.Money.Shell.Web.Extensions;
using System.IO;
using PVT.Money.Business.Management;

namespace PVT.Money.Shell.Web.Controllers
{
    [Documentation("Обрабатывает события вьюшек :)")]
    [Authorize]
    public class MainController : Controller
    {
        private IMoneyContainer container { get; set; }
        private readonly IMoneyUserManager _moneyUserManager;
        private readonly IMoneyRoleManager _moneyRoleManager;
        private readonly ILogManager _logManager;
        private readonly IUserInfoManager _userInfoManager;
        private readonly IRegistration _registration;
        private readonly IAccountDatabaseManager _accDatabaseManager;
        private readonly IAuthentication _authentication;
        private readonly IPermissionManager _permissionManager;
        private readonly IMoneyImageParser _moneyImageParser;

        public MainController(IMoneyContainer container,
            IMoneyUserManager moneyUserManager,
            IMoneyRoleManager moneyRoleManager,
            ILogManager logManager,
            IRegistration registration,
            IUserInfoManager userInfoManager,
            IAccountDatabaseManager accDatabaseManager,
            IAuthentication authentication,
            IMoneyImageParser moneyImageParser,
            IPermissionManager permissionManager)
        {
            this.container = container;
            _moneyUserManager = moneyUserManager;
            _moneyRoleManager = moneyRoleManager;
            _logManager = logManager;
            _userInfoManager = userInfoManager;
            _registration = registration;
            _accDatabaseManager = accDatabaseManager;
            _authentication = authentication;
            _permissionManager = permissionManager;
            _moneyImageParser = moneyImageParser;
        }

        [HttpGet]
        public async Task<IActionResult> History(HistoryModel model)
        {
            DateTime DateTo = DateTime.Now;
            DateTime DateFrom = DateTo.Subtract(new TimeSpan(14, 0, 0, 0, 0));

            model.Events = User.IsInPermission(RolePermissions.ViewFullHistory) ?
                await _logManager.GetLogEventsAsync(DateFrom, DateTo) :
                await _logManager.GetLogEventsAsync(DateFrom, DateTo, User.Identity.Name);

            model.DateFrom = DateFrom;
            model.DateTo = DateTo;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> HistoryFormAction(HistoryModel model)
        {
            DateTime DateTo = model.DateTo;
            DateTime DateFrom = model.DateFrom;

            model.Events = User.IsInPermission(RolePermissions.ViewFullHistory) ?
                await _logManager.GetLogEventsAsync(DateFrom, DateTo) :
                await _logManager.GetLogEventsAsync(DateFrom, DateTo, User.Identity.Name);

            return View(nameof(MainController.History), model);
        }

        [HttpGet]
        [CustomAuthorize(RolePermissions.ManageUsers)]
        public async Task<IActionResult> UsersManagement(UsersManagementModel model)
        {
            model.Users = await _moneyUserManager.GetUsersAsync();
            model.Roles = await _moneyRoleManager.GetRolesAsync();

            return View(model);
        }

        [HttpPost]
        [CustomAuthorize(RolePermissions.ManageUsers)]
        public async Task<IActionResult> UsersManagementFormAction(UsersManagementModel model)
        {
            string message = await _moneyRoleManager.ChangeUserRole(model.SelectedUser, model.NewRole);
            ViewData["UsersManagementMessage"] = message;

            await _logManager.WriteAsync(User.Identity.Name, message);

            model.Users = await _moneyUserManager.GetUsersAsync();
            model.Roles = await _moneyRoleManager.GetRolesAsync();

            return View(nameof(MainController.UsersManagement), model);
        }

        [HttpGet]
        [CustomAuthorize(RolePermissions.ManageRoles)]
        public async Task<IActionResult> RolesManagement(RolesManagementModel model)
        {
            model.Roles = await _moneyRoleManager.GetRolesAsync();
            model.Permissions = await _permissionManager.GetPermissionsAsStringsAsync();

            return View(model);
        }

        [HttpPost]
        [CustomAuthorize(RolePermissions.ManageRoles)]
        public async Task<IActionResult> RolesManagementCreateAction(RolesManagementModel model)
        {
            var admin = _moneyRoleManager;
            if (await admin.CreateRole(model.NewRole))
            {
                await _logManager.WriteAsync(User.Identity.Name, $"New role '{model.NewRole}' was created.");

                model.Roles = await _moneyRoleManager.GetRolesAsync();
                model.Permissions = await _permissionManager.GetPermissionsAsStringsAsync();

                model.NewRole = "";

                return View(nameof(MainController.RolesManagement), model);
            }
            else
                throw new InvalidOperationException($"Trying to add '{model.NewRole}' role that already exists.");
        }

        [HttpPost]
        [CustomAuthorize(RolePermissions.ManageRoles)]
        public async Task<IActionResult> RolesManagementPermissionsAction(RolesManagementModel model)
        {
            IEnumerable<string> newPerms = (model.NewPermissions == null) ? new List<string>() : model.NewPermissions.Split(',').ToList();
            IEnumerable<string> permsToAdd = await _permissionManager.GetPermissionsToAddAsync(model.SelectedRole, newPerms);
            IEnumerable<string> permsToRemove = await _permissionManager.GetPermissionsToRemoveAsync(model.SelectedRole, newPerms);

            foreach (string perm in permsToAdd)
            {
                await _permissionManager.AddPermissionToRoleAsync(model.SelectedRole, perm);
            }

            foreach (string perm in permsToRemove)
            {
                await _permissionManager.RemovePermissionFromRoleAsync(model.SelectedRole, perm);
            }

            await _logManager.WriteRoleChangingAsync(User.Identity.Name, model.SelectedRole, newPerms.ToList(), permsToRemove.ToList(), permsToAdd.ToList());

            model.Roles = await _moneyRoleManager.GetRolesAsync();
            model.Permissions = await _permissionManager.GetPermissionsAsStringsAsync();

            model.NewRole = "";

            return View(nameof(MainController.RolesManagement), model);
        }

        [HttpPost]
        public async Task<JsonResult> RoleNameExists(string newRole)
        {
            return Json(!await _moneyRoleManager.RoleExistsAsync(newRole));
        }


        public JsonResult IntegrationJsonTest(SignInModel model)
        {
            return Json(model);
        }

        [HttpGet]
        public async Task<IActionResult> PersonalData(PersonalDataModel model)
        {
            UserInfo uInfo = await _userInfoManager.GetUserInfoAsync(User.Identity.Name);

            model.Address = uInfo.Address;
            model.BirthDay = uInfo.BirthDay;
            model.BirthMonth = uInfo.BirthMonth;
            model.BirthMonthStr = uInfo.BirthMonthStr;
            model.BirthYear = uInfo.BirthYear;
            model.Email = uInfo.Email;
            model.FirstName = uInfo.FirstName;
            model.Gender = uInfo.Gender;
            model.LastName = uInfo.LastName;
            model.Login = uInfo.Login;
            model.Phone = uInfo.Phone;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PersonalDataFormAction(PersonalDataModel model)
        {
            int size = 200;

            ImageCheckResult imgCheck = _moneyImageParser.CheckImage(model.Photo, size*1000);

            if (imgCheck == ImageCheckResult.MaxSizeError)
            {
                ViewData["ImageError"] = $"Photo wasn't changed. Maximum image size is {size}kb.";
            }

            if (imgCheck == ImageCheckResult.IsNotJpeg)
            {
                ViewData["ImageError"] = "Photo wasn't changed. Only jpeg image format supported.";
            }

            if (imgCheck == ImageCheckResult.Success)
            {
                await _moneyImageParser.SaveUserImage(model.Photo,User.Identity.Name);
            }           

            UserInfo uInfo = new UserInfo
            {
                Address = model.Address,
                BirthYear = model.BirthYear,
                BirthDay = model.BirthDay,
                BirthMonth = model.BirthMonth,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                Gender = model.Gender,
                Login = model.Login
            };

            await _userInfoManager.SetUserInfoAsync(uInfo, User.Identity.Name);
            await _logManager.WriteAsync(uInfo.Login, $"User '{uInfo.Login}' edited his personal data.");

            if (uInfo.Login != User.Identity.Name)
            {
                string name = uInfo.Login;
                await _logManager.WriteAsync(name, $"User '{User.Identity.Name}' renamed himself into '{name}'.");

                await _authentication.SignOutAsync();

                await _logManager.WriteAsync(name, $"User '{name}' signed out.");

                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            model.BirthMonthStr = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.BirthMonth);
            return View(nameof(MainController.PersonalData), model);
        }

        public async Task<FileContentResult> GetUserPhoto(string userName)
        {
            byte[] byteArray = await _moneyImageParser.GetUserImage(userName, false);
            return byteArray != null
                ? new FileContentResult(byteArray, "image/jpeg")
                : null;
        }

        public async Task<FileContentResult> GetUserAvatar(string userName)
        {
            byte[] byteArray = await _moneyImageParser.GetUserImage(userName, true);
            return byteArray != null
                ? new FileContentResult(byteArray, "image/jpeg")
                : null;
        }

        [HttpPost]
        public async Task<JsonResult> LoginExists(string login)
        {
            if (login == User.Identity.Name)
                return Json(true);
            return Json(!await _registration.LoginAlreadyExistsAsync(login));
        }

        [HttpPost]
        public async Task<JsonResult> EmailExists(string email)
        {
            if (email == await _moneyUserManager.GetUserEmailAsync(User.Identity.Name))
                return Json(true);
            return Json(!await _registration.EmailAlreadyExistsAsync(email));
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordFormAction(PersonalDataModel model)
        {
            UserInfo uInfo = await _userInfoManager.GetUserInfoAsync(User.Identity.Name);

            model.Address = uInfo.Address;
            model.BirthDay = uInfo.BirthDay;
            model.BirthMonth = uInfo.BirthMonth;
            model.BirthMonthStr = uInfo.BirthMonthStr;
            model.BirthYear = uInfo.BirthYear;
            model.Email = uInfo.Email;
            model.FirstName = uInfo.FirstName;
            model.Gender = uInfo.Gender;
            model.LastName = uInfo.LastName;
            model.Login = uInfo.Login;
            model.Phone = uInfo.Phone;

            if (!await _moneyUserManager.IsNotOAuth(User.Identity.Name))
            {
                ViewData["PassChangeClass"] = "text-danger";
                ViewData["PassChangeMessage"] = "Password change failed: authentication type error.";
                return View(nameof(MainController.PersonalData), model);
            }

            if (!await _authentication.CheckPasswordAsync(model.ChangePassword.CurrentPassword, User.Identity.Name))
            {
                ViewData["PassChangeClass"] = "text-danger";
                ViewData["PassChangeMessage"] = "Password change failed: wrong current password.";
                return View(nameof(MainController.PersonalData), model);
            }

            IdentityResult result = await _moneyUserManager.ChangePasswordAsync(model.ChangePassword.CurrentPassword, model.ChangePassword.Password, User.Identity.Name);

            if (!result.Succeeded)
            {
                ViewData["PassChangeClass"] = "text-danger";
                ViewData["PassChangeMessage"] = "Password change failed.";
                return View(nameof(MainController.PersonalData), model);
            }

            await _logManager.WriteAsync(User.Identity.Name, $"User '{User.Identity.Name}' changed his password.");

            ViewData["PassChangeClass"] = "text-success";
            ViewData["PassChangeMessage"] = "Password successfully changed.";
            return View(nameof(MainController.PersonalData), model);
        }
    }
}