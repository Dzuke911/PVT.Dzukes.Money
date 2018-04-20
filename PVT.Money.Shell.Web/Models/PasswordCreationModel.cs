using PVT.Money.Shell.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PVT.Money.Shell.Web.Models
{
    public class PasswordCreationModel
    {
        public string Code { get; set; }

        public string Login { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [PasswordValidate(8, 16)]
        public string Password { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Compare("Password", ErrorMessage = "Password does not match the confirm password")]
        [PasswordValidate(8, 16)]
        public string ConfirmPassword { get; set; }
    }
}
