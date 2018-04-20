using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Data
{
    [Table("Accounts")]
    public class AccountEntity
    {
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Column("AccountName")]
        public string AccountName { get; set; }

        [Column("Amount")]
        public decimal Amount { get; set; }

        [Column("Currency")]
        public string Currency { get; set; }

        [Column("IsCommission")]
        public bool IsCommission { get; set; }

        [Required]
        [Column("UserId")]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public ApplicationUser User { get; set; }
    }
}
