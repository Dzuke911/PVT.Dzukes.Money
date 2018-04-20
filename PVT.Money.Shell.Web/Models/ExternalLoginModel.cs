using Microsoft.AspNetCore.Mvc;
using PVT.Money.Shell.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PVT.Money.Shell.Web.Models
{
    public class ExternalLoginModel
    {
        [Required(ErrorMessage = "This field is required")]
        [LoginValidate(8, 16)]
        [Remote("LoginExists", "Account", HttpMethod = "POST", ErrorMessage = "This login already registered.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Remote("EmailExists", "Account", HttpMethod = "POST", ErrorMessage = "This email address already registered.")]
        public string Email { get; set; }
    }
}
