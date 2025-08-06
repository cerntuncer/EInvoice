using DatabaseAccessLayer.Enumerations;

namespace BusinessLogicLayer.Handler.LineOfInvoiceHandler
{
    public class GetLineOfInvoiceByIdHandleResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public long Id { get; set; }
        public long InvoiceId { get; set; }
        public long ProductAndServiceId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Status Status { get; set; }
    }
}
