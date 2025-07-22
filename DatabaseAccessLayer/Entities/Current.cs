using DatabaseAccessLayer.Enumerations;
using DatabaseAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessLayer.Entities
{
    [Table("Currents")]
    public class Current : BaseEntity
    {
        public CurrencyType CurrencyType { get; set; }

        public CurrentType CurrentType { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
       

    }
}
