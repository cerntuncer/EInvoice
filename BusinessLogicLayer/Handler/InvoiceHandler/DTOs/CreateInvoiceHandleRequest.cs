using BusinessLogicLayer.Handler.LineOfInvoiceHandler;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.InvoiceHandler.DTOs
{
    public class CreateInvoiceHandleRequest : IRequest<CreateInvoiceHandleResponse>
    {
        public InvoiceType Type { get; set; }
        public InvoiceSenario Senario { get; set; }
        public long CurrentId { get; set; }
        public long CustomerSupplierId { get; set; }
        public Status Status { get; set; }
        public ICollection<lineOfInvoices> lineOfInovices { get; set; }
    }
    public class lineOfInvoices
    {
        public long ProductAndServiceId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
