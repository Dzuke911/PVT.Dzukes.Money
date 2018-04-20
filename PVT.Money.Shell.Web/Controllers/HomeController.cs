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
using System;
using System.Diagnostics;

namespace PVT.Money.Shell.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IMoneyContainer container { get; set; }//just test
        private readonly ILogManager _logManager;
        private readonly IAccountDatabaseManager _accDatabaseManager;
        private readonly IPermissionManager _permissionManager;

        public HomeController(IMoneyContainer container,
            IMoneyRoleManager moneyRoleManager,
            ILogManager logManager,
            IAccountDatabaseManager accDatabaseManager,
            IPermissionManager permissionManager)
        {
            this.container = container;
            _logManager = logManager;
            _accDatabaseManager = accDatabaseManager;
            _permissionManager = permissionManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // just to test container
            ClassA a = container.Create<ClassA>();

            IndexModel model = new IndexModel();

            model.Accounts = await _accDatabaseManager.GetAccountsAsync(User.Identity.Name);

            return View(model);
        }

        [HttpPost]
        [CustomAuthorize(RolePermissions.PutToAccount)]
        public async Task<IActionResult> MakeDepositAccountFormAction(IndexModel model)
        {
            decimal amount;
            if (!decimal.TryParse(model.DepositAmount, out amount))
            {
                model.ActivityMessage = "Deposit operation failed: invalid amount.";
                model.ActivityMessageError = true;
            }

            AccountActivityResult result = await _accDatabaseManager.MakeDepositAsync(model.DepositAccountName, model.DepositCurrency, amount, User.Identity.Name);

            if (model == null)
                model = new IndexModel();

            if (result == AccountActivityResult.AccountNotSelected)
            {
                model.ActivityMessage = "Deposit operation failed: account not selected.";
                model.ActivityMessageError = true;
            }
            if (result == AccountActivityResult.Success)
                model.ActivityMessage = $"Deposit operation completed.";

            await _logManager.WriteAsync(User.Identity.Name, $"Deposit: {model.DepositAmount} {model.DepositCurrency} added to '{model.DepositAccountName}' account.");
            model.Accounts = await _accDatabaseManager.GetAccountsAsync(User.Identity.Name);

            return View(nameof(HomeController.Index), model);
        }

        [HttpPost]
        [CustomAuthorize(RolePermissions.Transact)]
        public async Task<IActionResult> TransactToAccountFormAction(IndexModel model)
        {
            TransactionCheckResult result = await _accDatabaseManager.TransactionAsync(model.TransactToAccount.AccountFromName,
                User.Identity.Name,
                model.TransactToAccount.AccountToName,
                User.Identity.Name,
                Convert.ToDecimal(model.TransactToAccount.Amount));

            if (model == null)
                model = new IndexModel();

            model.Accounts = await _accDatabaseManager.GetAccountsAsync(User.Identity.Name);

            if (result.IsSameAccs == true)
            {
                model.ActivityMessage = "Transaction failed: select different accounts.";
                model.ActivityMessageError = true;
                return View(nameof(HomeController.Index), model);
            }
            if (result.IsEnough == false)
            {
                model.ActivityMessage = "Transaction failed: not enough funds.";
                model.ActivityMessageError = true;
                return View(nameof(HomeController.Index), model);
            }

            await _logManager.WriteAsync(User.Identity.Name, $"Transaction: {result.SendAmount} {result.CurrFrom} subtracted from '{model.TransactToAccount.AccountFromName}' account ({result.Commission} {result.CurrFrom} commission).");
            await _logManager.WriteAsync(User.Identity.Name, $"Transaction: {result.Receive} {result.CurrTo} added to '{model.TransactToAccount.AccountToName}' account.");
            await _logManager.WriteAsync(result.CommissionOwner, $"Commission getting: {result.Commission} {result.CurrFrom} added to '{result.CommissionAccName}' account.");

            model.ActivityMessage = "Transaction complete.";
            return View(nameof(HomeController.Index), model);
        }

        [HttpPost]
        [CustomAuthorize(RolePermissions.CreateAccount)]
        public async Task<IActionResult> CreateAccountFormAction(IndexModel model)
        {
            await _accDatabaseManager.CreateAccountAsync(model.AccountName, model.Currency, User.Identity.Name);

            await _logManager.WriteAsync(User.Identity.Name, $"User '{User.Identity.Name}' created '{model.AccountName}' account with currency '{model.Currency}'.");

            if (model == null)
                model = new IndexModel();

            model.ActivityMessage = $"'{model.AccountName}' account created.";
            model.Accounts = await _accDatabaseManager.GetAccountsAsync(User.Identity.Name);

            return View(nameof(HomeController.Index), model);
        }

        [HttpPost]
        public async Task<JsonResult> AccountNameExists(string AccountName)
        {
            return Json(!await _accDatabaseManager.IsAccountExistsAsync(AccountName, User.Identity.Name));
        }

        [HttpPost]
        public async Task<JsonResult> TransactionCheck(string accfrom, string accto, string amount, int reqnum)
        {
            if (accfrom == accto)
                return Json(new TransactionCheckResult() { IsSameAccs = true, ReqNum = reqnum });

            decimal decAmount;
            if (!decimal.TryParse(amount, out decAmount))
                return Json(new TransactionCheckResult() { IsParsed = false, ReqNum = reqnum });

            TransactionCheckResult checkResult = await _accDatabaseManager.CheckTransaction(accfrom, User.Identity.Name, accto, User.Identity.Name, decAmount);

            checkResult.ReqNum = reqnum;

            return Json(checkResult);
        }

        [HttpPost]
        [CustomAuthorize(RolePermissions.WithdrawFromAccount)]
        public async Task<IActionResult> WithdrawFromAccountFormAction(IndexModel model)
        {
            decimal amount;
            if (!decimal.TryParse(model.Withdraw.Amount, out amount))
            {
                if (model == null)
                    model = new IndexModel();

                model.Accounts = await _accDatabaseManager.GetAccountsAsync(User.Identity.Name);

                return View(nameof(HomeController.Index), model);
            }

            AccountActivityResult result = await _accDatabaseManager.WithdrawAsync(model.Withdraw.AccountName, amount, User.Identity.Name);

            if (model == null)
                model = new IndexModel();

            model.Accounts = await _accDatabaseManager.GetAccountsAsync(User.Identity.Name);

            if (result == AccountActivityResult.AccountNotSelected)
            {
                model.ActivityMessage = "Withdraw operation failed: account not selected.";
                model.ActivityMessageError = true;
            }

            if (result == AccountActivityResult.NotEnoughFunds)
            {
                model.ActivityMessage = $"Withdraw operation failed: not enough funds on '{model.Withdraw.AccountName}' account.";
                model.ActivityMessageError = true;
            }
            if (result == AccountActivityResult.Success)
            {
                string curr = await _accDatabaseManager.GetAccountCurrencyAsync(model.Withdraw.AccountName, User.Identity.Name);
                await _logManager.WriteAsync(User.Identity.Name, $"{model.Withdraw.Amount} {curr} withdrawed from '{model.Withdraw.AccountName}' account.");
                model.ActivityMessage = $"Withdraw operation completed.";
            }

            return View(nameof(HomeController.Index), model);
        }

        [HttpPost]
        [CustomAuthorize(RolePermissions.DeleteAccount)]
        public async Task<IActionResult> DeleteAccountFormAction(IndexModel model)
        {
            AccDeletingResult result = await _accDatabaseManager.DeleteAccountAsync(model.DeleteAccountName, User.Identity.Name);

            if (model == null)
                model = new IndexModel();

            model.Accounts = await _accDatabaseManager.GetAccountsAsync(User.Identity.Name);

            if (result == AccDeletingResult.IsCommissionError)
            {
                model.ActivityMessage = "Commission account can`t be deleted.";
                model.ActivityMessageError = true;
            }

            if (result == AccDeletingResult.HasMoneyError)
            {
                model.ActivityMessage = $"Not empty account can`t be deleted. Withdaw first.";
                model.ActivityMessageError = true;
            }
            if (result == AccDeletingResult.Success)
            {
                await _logManager.WriteAsync(User.Identity.Name, $"'{model.DeleteAccountName}' account was deleted.");
                model.ActivityMessage = $"Account was deleted.";
            }

            return View(nameof(HomeController.Index), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
