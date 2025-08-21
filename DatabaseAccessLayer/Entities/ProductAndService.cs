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
	[Table("ProductsAndServices")]
	public class ProductAndService : BaseEntity
	{
		[Required]
		[MaxLength(50)]
		public string Name { get; set; }
		public  UnitType UnitType {  get; set; }
		[Column(TypeName = "decimal(18,2)")]
		public decimal price { get; set; }
		public long UserId { get; set; }
		[ForeignKey("UserId")]
		public User User { get; set; }


	}
}