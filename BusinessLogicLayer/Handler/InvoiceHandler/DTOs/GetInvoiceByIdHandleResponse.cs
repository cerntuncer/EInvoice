using DatabaseAccessLayer.Enumerations;

namespace BusinessLogicLayer.Handler.InvoiceHandler.DTOs
{
    public class GetInvoiceByIdHandleResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public long Id { get; set; }
        public InvoiceType Type { get; set; }
        public InvoiceSenario Senario { get; set; }
        public DateTime Date { get; set; }
        public int No { get; set; }
        public long CurrentId { get; set; }
        public long CustomerSupplierId { get; set; }
        public Status Status { get; set; }

        public List<LineOfInvoiceDto> Lines { get; set; } = new();
    }
    public class LineOfInvoiceDto
    {
        public long Id { get; set; }
        public long InvoiceId { get; set; }
        public long ProductAndServiceId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

}
