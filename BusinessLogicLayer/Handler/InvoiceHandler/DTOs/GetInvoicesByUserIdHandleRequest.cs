using MediatR;

namespace BusinessLogicLayer.Handler.InvoiceHandler.DTOs
{
    public class GetInvoicesByUserIdHandleRequest : IRequest<GetInvoicesByUserIdHandleResponse>
    {
        public long UserId { get; set; }
    }
}