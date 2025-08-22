using MediatR;

namespace BusinessLogicLayer.Handler.LineOfInvoiceHandler.DTOs
{
    public class UpdateLineOfInvoiceHandleRequest : IRequest<UpdateLineOfInvoiceHandleResponse>
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int VatRate { get; set; }
    }
}
