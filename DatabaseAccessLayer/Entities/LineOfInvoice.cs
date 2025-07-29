using DatabaseAccessLayer.Enumerations;
using DatabaseAccessLayer.Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace DatabaseAccessLayer.Entities
{
    [Table("LineOfInvoices")]
    public class LineOfInvoice : BaseEntity
    {
        public int Quantity { get; set; }

        public long ProductAndServiceId { get; set; }
        [ForeignKey("ProductAndServiceId")]
        public ProductAndService ProductAndService { get; set; }

        public long InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        public Status Status { get; set; }
    }

}

