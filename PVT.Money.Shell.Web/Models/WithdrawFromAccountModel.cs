using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PVT.Money.Shell.Web.Models
{
    public class WithdrawFromAccountModel
    {
        public string AccountName { get; set; }

        [Required(ErrorMessage = "Amount field is required")]
        [RegularExpression("^[0-9]{1,8}(,[0-9]{1,8})??$", ErrorMessage = "Enter a valid amount.")]
        public string Amount { get; set; }
    }
}
