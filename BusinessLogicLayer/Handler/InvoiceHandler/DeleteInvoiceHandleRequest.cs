using MediatR;

namespace BusinessLogicLayer.Handler.InvoiceHandler
{
    public class DeleteInvoiceHandleRequest : IRequest<DeleteInvoiceHandleResponse>
    {
        public long Id { get; set; }
    }

   
}
