using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.LineOfInvoiceHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.LineOfInvoiceHandler.Commands
{
    public class DeleteLineOfInvoiceHandle : IRequestHandler<DeleteLineOfInvoiceHandleRequest, DeleteLineOfInvoiceHandleResponse>
    {
        private readonly ILineOfInvoiceRepository _repository;

        public DeleteLineOfInvoiceHandle(ILineOfInvoiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<DeleteLineOfInvoiceHandleResponse> Handle(DeleteLineOfInvoiceHandleRequest request, CancellationToken cancellationToken)
        {
            var line = _repository.Find(request.Id);

            if (line == null || line.Status == Status.Passive)
            {
                return new DeleteLineOfInvoiceHandleResponse
                {
                    Error = true,
                    Message = "Fatura satırı bulunamadı."
                };
            }

            line.Status = Status.Passive;
            _repository.Update(line);

            return new DeleteLineOfInvoiceHandleResponse
            {
                Error = false,
                Message = "Fatura satırı silindi (pasife alındı)."
            };
        }
    }
}
