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

                // Mevcut satırları getir
                var existingLines = _lineRepository.Where(x => x.InvoiceId == invoice.Id).ToList();

                // Güncellenecek veya eklenecek satırlar
                foreach (var reqLine in request.lineOfInovices)
                {
                    if (reqLine.Id.HasValue && reqLine.Id.Value > 0)
                    {
                        // Güncelle
                        var toUpdate = existingLines.FirstOrDefault(l => l.Id == reqLine.Id.Value);
                        if (toUpdate != null)
                        {
                            toUpdate.ProductAndServiceId = reqLine.ProductAndServiceId;
                            toUpdate.Quantity = reqLine.Quantity;
                            toUpdate.UnitPrice = reqLine.UnitPrice;
                            _lineRepository.Update(toUpdate);
                        }
                        else
                        {
                            // ID gelmiş ama mevcutta yoksa güvenli olması adına ekle
                            var createReq = new CreateLineOfInvoiceHandleRequest
                            {
                                InvoiceId = invoice.Id,
                                ProductAndServiceId = reqLine.ProductAndServiceId,
                                Quantity = reqLine.Quantity,
                                UnitPrice = reqLine.UnitPrice
                            };
                            var r = await _mediator.Send(createReq);
                            if (r.Error) throw new Exception(r.Message);
                        }
                    }
                    else
                    {
                        // Yeni ekle
                        var createReq = new CreateLineOfInvoiceHandleRequest
                        {
                            InvoiceId = invoice.Id,
                            ProductAndServiceId = reqLine.ProductAndServiceId,
                            Quantity = reqLine.Quantity,
                            UnitPrice = reqLine.UnitPrice
                        };
                        var r = await _mediator.Send(createReq);
                        if (r.Error) throw new Exception(r.Message);
                    }
                }

                // Request'te olmayan mevcut satırları sil
                var requestIds = request.lineOfInovices.Where(x => x.Id.HasValue && x.Id.Value > 0).Select(x => x.Id.Value).ToHashSet();
                foreach (var ex in existingLines)
                {
                    if (!requestIds.Contains(ex.Id))
                    {
                        _lineRepository.Delete(ex);
                    }
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
