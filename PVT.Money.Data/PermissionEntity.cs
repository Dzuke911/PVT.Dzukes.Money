using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Data
{
    [Table("Permissions")]
    public class PermissionEntity
    {
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Column("Permission")]
        public string Permission { get; set; }

        [ForeignKey("PermissionID")]
        public ICollection<PermissionToRoleEntity> PermissionRoles { get; set; }
    }
}
