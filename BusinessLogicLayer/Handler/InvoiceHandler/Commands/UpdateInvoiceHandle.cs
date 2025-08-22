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
        private readonly ICurrentRepository _currentRepository;
        private readonly MyContext _context;
        private readonly IMediator _mediator;

        public UpdateInvoiceHandle(
            IInvoiceRepository invoiceRepository,
            ILineOfInvoiceRepository lineRepository,
            ICurrentRepository currentRepository,
            MyContext context,
            IMediator mediator)
        {
            _invoiceRepository = invoiceRepository;
            _lineRepository = lineRepository;
            _currentRepository = currentRepository;
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
                // Önce eski değerleri tut
                var previousType = invoice.Type;
                var previousCurrentId = invoice.CurrentId;
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
                var oldTotal = existingLines.Sum(l => l.UnitPrice * l.Quantity * (1 + (decimal)l.VatRate / 100m));
                var newTotal = request.lineOfInovices?.Sum(l => l.UnitPrice * l.Quantity * (1 + (decimal)l.VatRate / 100m)) ?? 0m;

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
                            toUpdate.VatRate = reqLine.VatRate;
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
                                UnitPrice = reqLine.UnitPrice,
                                VatRate = reqLine.VatRate
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
                            UnitPrice = reqLine.UnitPrice,
                            VatRate = reqLine.VatRate
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

                // Cari bakiye güncelle: önce önceki etkileri geri al, sonra yenisini uygula
                var prevCurrent = _currentRepository.Find(previousCurrentId);
                if (prevCurrent != null)
                {
                    if (previousType == InvoiceType.Purchase)
                        prevCurrent.Balance += oldTotal; // alış etkisini geri al
                    else if (previousType == InvoiceType.Sales)
                        prevCurrent.Balance -= oldTotal; // satış etkisini geri al
                    _currentRepository.Update(prevCurrent);
                }

                var newCurrent = _currentRepository.Find(invoice.CurrentId);
                if (newCurrent != null)
                {
                    if (request.Type == InvoiceType.Purchase)
                        newCurrent.Balance -= newTotal; // alışta para düşer
                    else if (request.Type == InvoiceType.Sales)
                        newCurrent.Balance += newTotal; // satışta para artar
                    _currentRepository.Update(newCurrent);
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