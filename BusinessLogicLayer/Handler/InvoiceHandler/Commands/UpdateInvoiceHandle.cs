using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.InvoiceHandler.DTOs;
using BusinessLogicLayer.Handler.LineOfInvoiceHandler;
using BusinessLogicLayer.Handler.LineOfInvoiceHandler.DTOs;
using DatabaseAccessLayer.Contexts;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.InvoiceHandler.Commands
{
    public class UpdateInvoiceHandle : IRequestHandler<UpdateInvoiceHandleRequest, UpdateInvoiceHandleResponse>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ILineOfInvoiceRepository _lineRepository;
        private readonly MyContext _context;
        private readonly IMediator _mediator;

        public UpdateInvoiceHandle(
            IInvoiceRepository invoiceRepository,
            ILineOfInvoiceRepository lineRepository,
            MyContext context,
            IMediator mediator)
        {
            _invoiceRepository = invoiceRepository;
            _lineRepository = lineRepository;
            _context = context;
            _mediator = mediator;
        }

        public async Task<UpdateInvoiceHandleResponse> Handle(UpdateInvoiceHandleRequest request, CancellationToken cancellationToken)
        {
            var invoice = _invoiceRepository.Find(request.Id);
            if (invoice == null || invoice.Status == Status.Passive)
            {
                return new UpdateInvoiceHandleResponse
                {
                    Error = true,
                    Message = "Fatura bulunamadı veya silinmiş."
                };
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Fatura bilgilerini güncelle
                invoice.Type = request.Type;
                invoice.Senario = request.Senario;
                invoice.CustomerSupplierId = request.CustomerSupplierId;
                invoice.CurrentId = request.CurrentId;
                invoice.Status = request.Status;
                invoice.Date = DateTime.UtcNow;

                _invoiceRepository.Update(invoice);

                // Önce eski satırları sil
                var existingLines = _lineRepository.Where(x => x.InvoiceId == invoice.Id).ToList();
                foreach (var item in existingLines)
                {
                    _lineRepository.Delete(item);
                }

                // Yeni satırları ekle
                foreach (var line in request.lineOfInovices)
                {
                    var createLineRequest = new CreateLineOfInvoiceHandleRequest
                    {
                        InvoiceId = invoice.Id,
                        ProductAndServiceId = line.ProductAndServiceId,
                        Quantity = line.Quantity,
                        UnitPrice = line.UnitPrice
                    };

                    var result = await _mediator.Send(createLineRequest);
                    if (result.Error)
                        throw new Exception(result.Message);
                }

                await transaction.CommitAsync();

                return new UpdateInvoiceHandleResponse
                {
                    Error = false,
                    Message = "Fatura ve satırları başarıyla güncellendi."
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new UpdateInvoiceHandleResponse
                {
                    Error = true,
                    Message = $"Fatura güncelleme hatası: {ex.Message}"
                };
            }
        }
    }
}
