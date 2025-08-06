using MediatR;

namespace BusinessLogicLayer.Handler.LineOfInvoiceHandler
{
    public class UpdateLineOfInvoiceHandleRequest : IRequest<UpdateLineOfInvoiceHandleResponse>
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
