using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PVT.Money.Shell.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PVT.Money.Shell.Web.Models
{
    public class PersonalDataModel
    {
        [Required(ErrorMessage = "Login is required. ")]
        [LoginValidate(8, 16)]
        [Remote("LoginExists", "Main", HttpMethod = "POST", ErrorMessage = "This login already registered. ")]
        public string Login { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required. ")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address. ")]
        [Remote("EmailExists", "Main", HttpMethod = "POST", ErrorMessage = "This email address already registered. ")]
        public string Email { get; set; }

        [Range(1900, 2018, ErrorMessage = "Enter a valid birth year. ")]
        public int BirthYear { get; set; }

        public int BirthMonth { get; set; }

        public string BirthMonthStr { get; set; }

        public int BirthDay { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public ChangePasswordModel ChangePassword {get; set;}

        public IFormFile Photo { get; set; }
    }
}
