using Microsoft.AspNetCore.Mvc;
using PVT.Money.Business.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PVT.Money.Shell.Web.Models
{
    public class RolesManagementModel
    {
        public IEnumerable<UserRole> Roles;
        public IEnumerable<string> Permissions;

        [RegularExpression("^[a-zA-Z0-9_-]+$", ErrorMessage = "Role name has to contain alphanumeric, '-' or '_' characters only")]
        [StringLength(20,ErrorMessage = "Role name has to be between 3 and 20 characters.",MinimumLength = 3)]
        [Remote("RoleNameExists", "Main", HttpMethod = "POST", ErrorMessage = "This role already exists.")]
        public string NewRole { get; set; }

        public string SelectedRole { get; set; }
        public string NewPermissions { get; set; }
    }
}
