using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.LineOfInvoiceHandler.DTOs;
using MediatR;

namespace BusinessLogicLayer.Handler.LineOfInvoiceHandler.Queries
{
    public class GetLineOfInvoiceByIdHandle : IRequestHandler<GetLineOfInvoiceByIdHandleRequest, GetLineOfInvoiceByIdHandleResponse>
    {
        private readonly ILineOfInvoiceRepository _lineRepository;

        public GetLineOfInvoiceByIdHandle(ILineOfInvoiceRepository lineRepository)
        {
            _lineRepository = lineRepository;
        }

        public async Task<GetLineOfInvoiceByIdHandleResponse> Handle(GetLineOfInvoiceByIdHandleRequest request, CancellationToken cancellationToken)
        {
            var line = _lineRepository.GetById(request.Id);

            if (line == null)
            {
                return new GetLineOfInvoiceByIdHandleResponse
                {
                    Error = true,
                    Message = "Fatura satırı bulunamadı."
                };
            }

            return new GetLineOfInvoiceByIdHandleResponse
            {
                Error = false,
                Message = "Fatura satırı başarıyla getirildi.",
                Id = line.Id,
                InvoiceId = line.InvoiceId,
                ProductAndServiceId = line.ProductAndServiceId,
                Quantity = line.Quantity,
                UnitPrice = line.UnitPrice,
                Status = line.Status
            };
        }
    }
}
