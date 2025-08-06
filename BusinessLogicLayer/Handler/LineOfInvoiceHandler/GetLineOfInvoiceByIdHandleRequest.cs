using MediatR;

namespace BusinessLogicLayer.Handler.LineOfInvoiceHandler
{
    public class GetLineOfInvoiceByIdHandleRequest : IRequest<GetLineOfInvoiceByIdHandleResponse>
    {
        public long Id { get; set; }
    }

}
