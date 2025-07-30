using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.LineOfInvoiceHandler
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
            if (request == null)
                return new CreateLineOfInvoiceHandleResponse { Message = "İstek boş olamaz.", Error = true };

            if (request.InvoiceId <= 0)
                return new CreateLineOfInvoiceHandleResponse { Message = "Geçersiz fatura ID.", Error = true };

            if (request.ProductAndServiceId <= 0)
                return new CreateLineOfInvoiceHandleResponse { Message = "Geçersiz ürün/hizmet ID.", Error = true };

            if (request.Quantity <= 0)
                return new CreateLineOfInvoiceHandleResponse { Message = "Miktar sıfırdan büyük olmalı.", Error = true };

            if (request.UnitPrice < 0)
                return new CreateLineOfInvoiceHandleResponse { Message = "Birim fiyat negatif olamaz.", Error = true };

            // 🔍 Aktiflik kontrolleri
            var invoice = _invoiceRepository.Find(request.InvoiceId);
            if (invoice == null || invoice.Status != Status.Active)
                return new CreateLineOfInvoiceHandleResponse { Message = "Fatura bulunamadı veya pasif.", Error = true };

            var product = _productRepository.Find(request.ProductAndServiceId);
            if (product == null || product.Status != Status.Active)
                return new CreateLineOfInvoiceHandleResponse { Message = "Ürün/Hizmet bulunamadı veya pasif.", Error = true };

            // 🧾 Satır oluştur
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
                Message = "Fatura satırı başarıyla oluşturuldu.",
                Error = false
            };
        }
    }
}
