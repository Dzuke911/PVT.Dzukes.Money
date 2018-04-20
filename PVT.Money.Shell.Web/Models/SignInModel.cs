using PVT.Money.Shell.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PVT.Money.Shell.Web.Models
{
    public class SignInModel
    {
        [Required(ErrorMessage = "This field is required")]
        public string Login { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
