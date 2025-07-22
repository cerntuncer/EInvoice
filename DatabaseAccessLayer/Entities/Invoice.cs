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
    [Table("Invoices")]
    public class Invoice : BaseEntity
    {
        public InvoiceType Type { get; set; }
        public int No { get; set; }
        public DateTime Date { get; set; }
        public InvoiceSenario Senario { get; set; }
        public long CurrentId { get; set; }
        [ForeignKey("CurrentId")]
        public Current Current { get; set; }//faturada hangi kasa ya da bankadan giriş çıkış oldu 
        public long CustomerSupplierId { get; set; }
        [ForeignKey("CustomerSupplierId")]
        public CustomerSupplier CustomerSupplier { get; set; } 
        public ICollection<LineOfInvoice> LineOfInvoices { get; set; } = new List<LineOfInvoice>();
    }
}
