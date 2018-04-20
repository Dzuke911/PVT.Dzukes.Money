using PVT.Money.Shell.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PVT.Money.Shell.Web.Models
{
    public class TransactToAccountModel
    {
        public string AccountFromName { get; set; }

        [Required(ErrorMessage ="Amount field is required")]
        [RegularExpression("^[0-9]{1,8}(,[0-9]{1,8})??$", ErrorMessage = "Enter a valid amount.")]
        public string Amount { get; set; }

        [CompareFalse("AccountFromName",ErrorMessage ="Select two different accounts for transaction.")]
        public string AccountToName { get; set; }
    }
}
