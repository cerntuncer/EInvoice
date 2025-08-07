using MediatR;

namespace BusinessLogicLayer.Handler.LineOfInvoiceHandler.DTOs
{
    public class DeleteLineOfInvoiceHandleRequest : IRequest<DeleteLineOfInvoiceHandleResponse>
    {
        public long Id { get; set; }
    }
}
