using MediatR;

namespace BusinessLogicLayer.Handler.InvoiceHandler.DTOs
{
    public class GetInvoiceByIdHandleRequest : IRequest<GetInvoiceByIdHandleResponse>
    {
        public long Id { get; set; }
    }

}
