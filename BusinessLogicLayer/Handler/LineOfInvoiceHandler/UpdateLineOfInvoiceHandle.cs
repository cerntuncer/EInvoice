using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.LineOfInvoiceHandler
{
    public class UpdateLineOfInvoiceHandle : IRequestHandler<UpdateLineOfInvoiceHandleRequest, UpdateLineOfInvoiceHandleResponse>
    {
        private readonly ILineOfInvoiceRepository _repository;

        public UpdateLineOfInvoiceHandle(ILineOfInvoiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<UpdateLineOfInvoiceHandleResponse> Handle(UpdateLineOfInvoiceHandleRequest request, CancellationToken cancellationToken)
        {
            var line = _repository.Find(request.Id);

            if (line == null || line.Status == Status.Passive)
            {
                return new UpdateLineOfInvoiceHandleResponse
                {
                    Error = true,
                    Message = "Fatura satırı bulunamadı."
                };
            }

            line.Quantity = request.Quantity;
            line.UnitPrice = request.UnitPrice;

            _repository.Update(line);

            return new UpdateLineOfInvoiceHandleResponse
            {
                Error = false,
                Message = "Fatura satırı güncellendi."
            };
        }
    }
}
