using MediatR;

namespace BusinessLogicLayer.Handler.LineOfInvoiceHandler
{
    public class DeleteLineOfInvoiceHandleRequest : IRequest<DeleteLineOfInvoiceHandleResponse>
    {
        public long Id { get; set; }
    }
}
