using MediatR;

namespace BusinessLogicLayer.Handler.InvoiceHandler.DTOs
{
    public class DeleteInvoiceHandleRequest : IRequest<DeleteInvoiceHandleResponse>
    {
        public long Id { get; set; }
    }

   
}
