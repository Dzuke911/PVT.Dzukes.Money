using Microsoft.AspNetCore.Mvc;
using PVT.Money.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PVT.Money.Shell.Web.Models
{
    public class IndexModel
    {
        [Required(ErrorMessage = "'Account name' field is required.")]
        [Remote("AccountNameExists", "Home", HttpMethod = "POST", ErrorMessage = "Account with this name already exists.")]
        [RegularExpression("^[a-zA-Z0-9_]+$", ErrorMessage = "Only alphanumeric and '_' characters allowed.")]
        public string AccountName { get; set; }

        public string Currency { get; set; }

        public IEnumerable<Account> Accounts { get; set; }

        [Required(ErrorMessage = "'Deposit amount' field is required.")]
        [RegularExpression("^[0-9]{1,8}(,[0-9]{1,8})??$", ErrorMessage = "Enter a valid amount. (format example: 123,456)")]
        public string DepositAmount { get; set; }

        public string DepositAccountName { get; set; }

        public string DepositCurrency { get; set; }

        public TransactToAccountModel TransactToAccount { get; set; }

        public WithdrawFromAccountModel Withdraw { get; set; }

        public string ActivityMessage { get; set; }

        public bool ActivityMessageError { get; set; }

        public string DeleteAccountName { get; set; }
    }
}
