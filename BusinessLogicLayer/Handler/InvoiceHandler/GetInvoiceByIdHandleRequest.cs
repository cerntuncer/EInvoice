using MediatR;

namespace BusinessLogicLayer.Handler.InvoiceHandler
{
    public class GetInvoiceByIdHandleRequest : IRequest<GetInvoiceByIdHandleResponse>
    {
        public long Id { get; set; }
    }

}
