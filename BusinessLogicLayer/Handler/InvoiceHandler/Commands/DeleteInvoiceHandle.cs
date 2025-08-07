using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.InvoiceHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.InvoiceHandler.Commands
{
    public class DeleteInvoiceHandle : IRequestHandler<DeleteInvoiceHandleRequest, DeleteInvoiceHandleResponse>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ILineOfInvoiceRepository _lineRepository;

        public DeleteInvoiceHandle(IInvoiceRepository invoiceRepository, ILineOfInvoiceRepository lineRepository)
        {
            _invoiceRepository = invoiceRepository;
            _lineRepository = lineRepository;
        }

        public async Task<DeleteInvoiceHandleResponse> Handle(DeleteInvoiceHandleRequest request, CancellationToken cancellationToken)
        {
            var invoice = _invoiceRepository.Find(request.Id);

            if (invoice == null || invoice.Status == Status.Passive)
            {
                return new DeleteInvoiceHandleResponse
                {
                    Error = true,
                    Message = "Fatura bulunamadı veya zaten silinmiş."
                };
            }

            //Faturaya ait satırları pasife al
            var relatedLines = _lineRepository.Where(x => x.InvoiceId == request.Id).ToList();
            foreach (var line in relatedLines)
            {
                line.Status = Status.Passive;
                _lineRepository.Update(line);
            }

            //Fatura da pasife alınır
            invoice.Status = Status.Passive;
            _invoiceRepository.Update(invoice);

            return new DeleteInvoiceHandleResponse
            {
                Error = false,
                Message = "Fatura ve bağlı satırları silindi (pasife alındı)."
            };
        }
    }
}
