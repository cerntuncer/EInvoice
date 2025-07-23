using DatabaseAccessLayer.Enumerations;
using DatabaseAccessLayer.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
namespace DatabaseAccessLayer.Entities
{
    [Table("Person")]
    public class Person : BaseEntity
    {

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } 
        public long IdentityNumber { get; set; }
        [MaxLength(150)]
        public string TaxOffice { get; set; }
        public PersonType Type { get; set; }
        public Status Status { get; set; }
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
 