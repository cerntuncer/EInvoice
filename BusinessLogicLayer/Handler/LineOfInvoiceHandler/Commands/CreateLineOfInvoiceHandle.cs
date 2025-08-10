using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.LineOfInvoiceHandler.DTOs;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.LineOfInvoiceHandler.Commands
{
    public class CreateLineOfInvoiceHandle : IRequestHandler<CreateLineOfInvoiceHandleRequest, CreateLineOfInvoiceHandleResponse>
    {
        private readonly ILineOfInvoiceRepository _lineRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IProductAndServiceRepository _productRepository;

        public CreateLineOfInvoiceHandle(
            ILineOfInvoiceRepository lineRepository,
            IInvoiceRepository invoiceRepository,
            IProductAndServiceRepository productRepository)
        {
            _lineRepository = lineRepository;
            _invoiceRepository = invoiceRepository;
            _productRepository = productRepository;
        }

        public async Task<CreateLineOfInvoiceHandleResponse> Handle(CreateLineOfInvoiceHandleRequest request, CancellationToken cancellationToken)
        {
            string? message = null;

            // --- Temel validasyonlar ---
            if (request == null)
                message = "İstek boş olamaz.";
            else if (request.InvoiceId <= 0)
                message = "Geçersiz fatura ID.";
            else if (request.ProductAndServiceId <= 0)
                message = "Geçersiz ürün/hizmet ID.";
            else if (request.Quantity <= 0)
                message = "Miktar sıfırdan büyük olmalı.";
            else if (request.UnitPrice < 0)
                message = "Birim fiyat negatif olamaz.";

            // --- Varlık ve durum kontrolleri ---
            if (message == null)
            {
                var invoice = _invoiceRepository.Find(request.InvoiceId);
                if (invoice == null || invoice.Status == Status.Passive)
                    message = "Fatura bulunamadı veya pasif.";

                if (message == null)
                {
                    var product = _productRepository.Find(request.ProductAndServiceId);
                    if (product == null || product.Status != Status.Active)
                        message = "Ürün/Hizmet bulunamadı veya pasif.";
                }
            }

            if (message != null)
            {
                return new CreateLineOfInvoiceHandleResponse
                {
                    Error = true,
                    Message = message
                };
            }

            // --- Satır oluştur ---
            var line = new LineOfInvoice
            {
                InvoiceId = request.InvoiceId,
                ProductAndServiceId = request.ProductAndServiceId,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice,
                Status = Status.Active
            };

            _lineRepository.Add(line);

            return new CreateLineOfInvoiceHandleResponse
            {
                Error = false,
                Message = "Fatura satırı başarıyla oluşturuldu."
            };
        }
    }
}
