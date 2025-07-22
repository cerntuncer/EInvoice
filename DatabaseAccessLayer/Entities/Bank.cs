using DatabaseAccessLayer.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DatabaseAccessLayer.Entities
{
    [Table("Banks")]
    public class Bank : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(26)]
        public string Iban { get; set; }
        public int BranchCode {  get; set; }  
        public int AccountNo { get; set; }
        public long CurrentId { get; set; }

        [ForeignKey("CurrentId")]
        public Current Current { get; set; }
    }
}
