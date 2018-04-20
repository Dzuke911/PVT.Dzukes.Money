using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PVT.Money.Business.Enums;
using PVT.Money.Business.Extensions;
using PVT.Money.Business.MoneyClasses.ForGlobalRates;
using PVT.Money.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.Main
{
    public class AccountDatabaseManager : IAccountDatabaseManager
    {
        protected DatabaseContext DBContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGlobalRates _globalRates;

        internal AccountDatabaseManager(DatabaseContext dbContext, UserManager<ApplicationUser> userManager, IGlobalRates globalRates)
        {
            DBContext = dbContext;
            _userManager = userManager;
            _globalRates = globalRates;
        }

        public async Task CreateAccountAsync(string accName, string currency, string userName)
        {
            ApplicationUser user = await _userManager.GetUserAsync(userName);

            if (!await IsAccountExistsAsync(accName, userName))
            {
                AccountEntity account = new AccountEntity() { AccountName = accName, Amount = 0.00m, Currency = currency, IsCommission = false, UserID = user.Id };

                await DBContext.Accounts.AddAsync(account);
                await DBContext.SaveChangesAsync();
            }
        }

        public async Task<AccountActivityResult> MakeDepositAsync(string accName, string currency, decimal amount, string userName)
        {
            if (accName == null)
                return AccountActivityResult.AccountNotSelected;

            AccountEntity account = await GetAccountEntityAsync(accName, userName);

            if(account == null)
                throw new InvalidOperationException($"There is no '{accName}' account owned by '{userName}' user.");

            AccountManager accManager = new AccountManager(0m, Currency.USD);

            Account acc = new Account(account, AccountCreationOptions.EmptyName);

            Account donor = new Account("", amount, currency.StrToCurrency());

            decimal coefficient = await _globalRates.GetRateAsync(donor.Money.Curr, acc.Money.Curr);
            acc.Money.AdjustExchangeRate(donor.Money.Curr, acc.Money.Curr, coefficient);
            donor.Money.AdjustExchangeRate(donor.Money.Curr, acc.Money.Curr, coefficient);

            accManager.Transaction(donor, acc, amount);

            account.Amount = acc.Money.Amount;

            DBContext.Accounts.Update(account);
            await DBContext.SaveChangesAsync();

            return AccountActivityResult.Success;
        }

        public async Task<bool> IsAccountExistsAsync(string accName, string userName)
        {
            ApplicationUser user = await _userManager.GetUserAsync(userName);

            await DBContext.Entry(user).Collection(u => u.Accounts).LoadAsync();

            AccountEntity result = user.Accounts.SingleOrDefault(a => a.AccountName == accName);

            return result != null;
        }

        public async Task<IEnumerable<Account>> GetAccountsAsync(string userName)
        {
            ApplicationUser user = await _userManager.GetUserAsync(userName);

            await DBContext.Entry(user).Collection(u => u.Accounts).LoadAsync();

            List<Account> result = new List<Account>();

            foreach(AccountEntity acc in user.Accounts)
            {
                result.Add(new Account(acc));
            }

            return result;
        }

        public async Task<TransactionCheckResult> TransactionAsync(string accFromName, string userFromName, string accToName, string userToName, decimal amount)
        {
            TransactionCheckResult result = new TransactionCheckResult();

            if(accFromName == accToName && userFromName == userToName)
            {
                result.IsSameAccs = true;
                return result;
            }

            AccountEntity accEntityFrom = await GetAccountEntityAsync(accFromName, userFromName);
            AccountEntity accEntityTo = await GetAccountEntityAsync(accToName, userToName);

            Account accFrom = new Account(accEntityFrom, AccountCreationOptions.EmptyName);
            Account accTo = new Account(accEntityTo, AccountCreationOptions.EmptyName);

            decimal coefficient = await _globalRates.GetRateAsync(accFrom.Money.Curr, accTo.Money.Curr);
            accFrom.Money.AdjustExchangeRate(accFrom.Money.Curr, accTo.Money.Curr, coefficient);
            accTo.Money.AdjustExchangeRate(accFrom.Money.Curr, accTo.Money.Curr, coefficient);

            AccountManager accManager = await GetAccountManagerAsync(accEntityFrom, userFromName);

            AccountEntity commissionAccEntity = await GetCommissionAccount();
            CommissionAccount commissionAcc = new CommissionAccount(commissionAccEntity, AccountCreationOptions.CurrencyOnly);

            commissionAcc.SubscribeToCommission(accManager);

            try
            {
                accManager.Transaction(accFrom, accTo, amount);
            }
            catch(Exception ex)
            {
                if (ex.Message == ExceptionMessages.TransactionInsufficientFunds())
                {
                    result.IsEnough = false;
                    return result;
                }
                else throw new Exception(ex.Message);
            }
            
            commissionAcc.UnsubscribeFromCommission(accManager);

            result.IsEnough = true;
            result.SendAmount = accEntityFrom.Amount - accFrom.Money.Amount;
            result.Commission = result.SendAmount - amount;
            result.Receive = accTo.Money.Amount - accEntityTo.Amount;
            result.CommissionOwner = commissionAccEntity.User.UserName;
            result.CommissionAccName = commissionAccEntity.AccountName;
            result.CurrFrom = accEntityFrom.Currency;
            result.CurrTo = accEntityTo.Currency;

            accEntityFrom.Amount = accFrom.Money.Amount;
            accEntityTo.Amount = accTo.Money.Amount;
            commissionAccEntity.Amount += commissionAcc.Money.Amount;

            DBContext.Accounts.Update(accEntityFrom);
            DBContext.Accounts.Update(accEntityTo);
            DBContext.Accounts.Update(commissionAccEntity);

            await DBContext.SaveChangesAsync();

            return result;
        }

        private async Task<AccountEntity> GetAccountEntityAsync(string accName, string userName)
        {
            ApplicationUser user = await _userManager.GetUserAsync(userName);

            await DBContext.Entry(user).Collection(u => u.Accounts).LoadAsync();

            AccountEntity result = user.Accounts.FirstOrDefault(a => a.AccountName == accName);            

            if(result == null)
                throw new InvalidOperationException($"User '{userName}' hasn`t account with '{accName}' name.");

            await DBContext.Entry(result).Reference(a => a.User).LoadAsync();

            return result;
        }

        public async Task<TransactionCheckResult> CheckTransaction(string accFromName, string userFromName, string accToName, string userToName, decimal amount)
        {
            decimal commissionCoeff = 0;
            TransactionCheckResult result = new TransactionCheckResult() { IsParsed = true };

            AccountEntity accEntityFrom = await GetAccountEntityAsync(accFromName, userFromName);
            AccountEntity accEntityTo = await GetAccountEntityAsync(accToName, userToName);

            result.CurrFrom = accEntityFrom.Currency;
            result.CurrTo = accEntityTo.Currency;

            if (accEntityFrom.IsCommission != true)
                commissionCoeff = 0.02m;

            Money moneyFrom = new Money(accEntityFrom.Amount, accEntityFrom.Currency.StrToCurrency());

            if (!moneyFrom.IsEnoughMoney(new Money(amount, accEntityFrom.Currency.StrToCurrency()), new Money(amount* commissionCoeff, accEntityFrom.Currency.StrToCurrency())))
            {
                result.IsEnough = false;
                result.SendAmount = accEntityFrom.Amount / (1m + commissionCoeff);
            }
            else
            {
                decimal exchangeCoeff = await _globalRates.GetRateAsync(accEntityFrom.Currency.StrToCurrency(), accEntityTo.Currency.StrToCurrency());
                result.IsEnough = true;
                result.SendAmount = amount * (1m + commissionCoeff);
                result.Receive = amount * exchangeCoeff;
                result.Commission = amount * commissionCoeff;
                result.Rest = accEntityFrom.Amount - result.SendAmount;
            }
            return result;
        }

        public async Task<AccountActivityResult> WithdrawAsync(string accName, decimal amount, string userName)
        {
            if (accName == null)
                return AccountActivityResult.AccountNotSelected;

            AccountEntity account = await GetAccountEntityAsync(accName, userName);

            if (account == null)
                throw new InvalidOperationException($"There is no '{accName}' account owned by '{userName}' user.");

            if (amount > account.Amount)
                return AccountActivityResult.NotEnoughFunds;

            account.Amount -= amount;

            DBContext.Accounts.Update(account);
            await DBContext.SaveChangesAsync();

            return AccountActivityResult.Success;
        }

        public async Task<AccountEntity> GetCommissionAccount()
        {
            return await GetAccountEntityAsync("Commission_account", "Dzuke911");
        }

        public async Task<AccountManager> GetAccountManagerAsync(AccountEntity accFrom, string userFromName)
        {
            if (accFrom.IsCommission == true)
                return await Task.FromResult(new AccountManager(0, Currency.USD));
            else
                return await Task.FromResult(new AccountManager(0.02m, accFrom.Currency.StrToCurrency()));
        }

        public async Task<string> GetAccountCurrencyAsync(string accName, string userName)
        {
            AccountEntity acc = await GetAccountEntityAsync(accName, userName);
            return acc.Currency;
        }

        public async Task<AccDeletingResult> DeleteAccountAsync(string accName, string userName)
        {
            ApplicationUser user = await _userManager.GetUserAsync(userName);

            AccountEntity account = await GetAccountEntityAsync(accName, userName);

            if (account.IsCommission == true)
                return AccDeletingResult.IsCommissionError;

            if (account.Amount >= 0.01m)
                return AccDeletingResult.HasMoneyError;

            DBContext.Accounts.Remove(account);
            await DBContext.SaveChangesAsync();

            return AccDeletingResult.Success;
        }
    }
}
