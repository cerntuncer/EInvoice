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
    [Table("Cases")]
    public class Case : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Address {  get; set; }

        public long CurrentId { get; set; }

        [ForeignKey("CurrentId")]
        public Current Current { get; set; }
      
    }
}
