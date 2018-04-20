using PVT.Money.Business.Enums;
using PVT.Money.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.Main
{
    public interface IAccountDatabaseManager
    {
        Task CreateAccountAsync(string accName, string currency, string userName);
        Task<bool> IsAccountExistsAsync(string accName, string userName);
        Task<IEnumerable<Account>> GetAccountsAsync(string userName);
        Task<AccountActivityResult> MakeDepositAsync(string accName, string currency, decimal amount, string userName);
        Task<TransactionCheckResult> TransactionAsync(string accFromName, string userFromName, string accToName, string userToName, decimal amount);
        Task<TransactionCheckResult> CheckTransaction(string accFromName, string userFromName, string accToName, string userToName, decimal amount);
        Task<AccountActivityResult> WithdrawAsync(string accName, decimal amount, string userName);
        Task<string> GetAccountCurrencyAsync(string accName, string userName);
        Task<AccDeletingResult> DeleteAccountAsync(string accName, string userName);
    }
}
