using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Data
{
    [Table("LogEvents")]
    public class LogEventEntity
    {
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Column("Event")]
        public string Event { get; set; }

        [Required]
        [Column("Date")]
        public DateTime Date { get; set; }

        [Required]
        [Column("UserId")]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public ApplicationUser User { get; set; }
    }
}
