using DatabaseAccessLayer.Enumerations;
using DatabaseAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessLayer.Entities
{
    [Table("CustomersSuppliers")]
    public class CustomerSupplier : BaseEntity
    {
        public CustomerOrSupplierType Type { get; set; }
        public long PersonId { get; set; }
        [ForeignKey("PersonId")]
        public Person Person { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

    }
}