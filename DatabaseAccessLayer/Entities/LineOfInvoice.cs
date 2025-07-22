using DatabaseAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessLayer.Entities
{
    [Table("LineOfInvoices")]
    public class LineOfInvoice : BaseEntity
    {
        public int Quantity { get; set; }
        public  long ProductId {  get; set; }
        [ForeignKey("ProductId")]
        public ProductAndService ProductAndService { get; set; }
        public long InvoiceID { get; set; }

        [ForeignKey("InvoiceID")]
        public Invoice Invoice { get; set; }
    }
}
