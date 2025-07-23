using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAccessLayer.Enumerations;
using DatabaseAccessLayer.Models;

namespace DatabaseAccessLayer.Entities
{
    [Table("Users")]
    public class User :BaseEntity
    {
        public Status Status { get; set; }
        public CustomerOrSupplierType Type { get; set; }
        public long PersonId {  get; set; }
        [ForeignKey("PersonId")]
        public ICollection<Current> Current { get; set; } = new List<Current>();

    }
}
