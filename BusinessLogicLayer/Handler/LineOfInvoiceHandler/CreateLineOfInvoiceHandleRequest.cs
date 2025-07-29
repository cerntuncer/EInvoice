using MediatR;

namespace BusinessLogicLayer.Handler.LineOfInvoiceHandler
{
    public class CreateLineOfInvoiceHandleRequest : IRequest<CreateLineOfInvoiceHandleResponse>
    {
        public long InvoiceId { get; set; }
        public long ProductAndServiceId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

}
