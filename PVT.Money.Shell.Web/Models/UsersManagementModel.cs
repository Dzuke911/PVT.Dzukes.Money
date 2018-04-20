using PVT.Money.Business;
using PVT.Money.Business.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PVT.Money.Shell.Web.Models
{
    public class UsersManagementModel
    {
        public IEnumerable<MoneyUser> Users;
        public IEnumerable<UserRole> Roles;

        [Required(ErrorMessage = "Select user before.")]
        public string SelectedUser { get; set; }
        public string NewRole { get; set; }
    }
}
