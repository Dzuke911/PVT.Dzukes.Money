using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Data
{
    [Table("PermissionsToRoles")]
    public class PermissionToRoleEntity
    {
        [Column("RoleId")]
        public int RoleID { get; set; }

        [Column("PermissionId")]
        public int PermissionID { get; set; }

        [ForeignKey(nameof(RoleID))]
        public ApplicationRole Role { get; set; }

        [ForeignKey(nameof(PermissionID))]
        public PermissionEntity Permission { get; set; }
    }
}
