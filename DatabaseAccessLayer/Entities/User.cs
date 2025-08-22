
using System.ComponentModel.DataAnnotations.Schema;

using DatabaseAccessLayer.Enumerations;
using DatabaseAccessLayer.Models;

namespace DatabaseAccessLayer.Entities
{
    [Table("Users")]
    public class User : BaseEntity
    {

        public UserType Type { get; set; }
        public long PersonId { get; set; }
        [ForeignKey("PersonId")]
        public Person Person { get; set; }
        public ICollection<Current> Current { get; set; } = new List<Current>();
        public ICollection<CustomerSupplier> CustomerSuppliers { get; set; } = new List<CustomerSupplier>();
        public UserCredential UserCredential { get; set; }





    }
}