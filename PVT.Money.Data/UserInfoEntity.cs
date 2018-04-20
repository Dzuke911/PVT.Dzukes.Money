using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Data
{
    [Table("UsersInfo")]
    public class UserInfoEntity
    {
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("FirstName")]
        public string FirstName { get; set; }

        [Column("LastName")]
        public string LastName { get; set; }

        [Column("BirthDate")]
        public DateTime? BirthDate { get; set; }

        [Column("Gender")]
        public string Gender { get; set; }

        [Column("Address")]
        public string Address { get; set; }

        [Column("Phone")]
        public string Phone { get; set; }

        [Column("Photo")]
        public byte[] Photo { get; set; }

        [Required]
        [Column("UserId")]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public ApplicationUser User { get; set; }
    }
}
