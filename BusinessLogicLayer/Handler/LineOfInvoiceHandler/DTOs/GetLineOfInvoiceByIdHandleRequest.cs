using MediatR;

namespace BusinessLogicLayer.Handler.LineOfInvoiceHandler.DTOs
{
    public class GetLineOfInvoiceByIdHandleRequest : IRequest<GetLineOfInvoiceByIdHandleResponse>
    {
        public long Id { get; set; }
    }

}
