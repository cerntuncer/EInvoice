using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Entities;
using MediatR;

namespace BusinessLogicLayer.Handler.LineOfInvoiceHandler
{
    public class CreateLineOfInvoiceHandle : IRequestHandler<CreateLineOfInvoiceHandleRequest, CreateLineOfInvoiceHandleResponse>
    {
        private readonly ILineOfInvoiceRepository _lineRepository;

        public CreateLineOfInvoiceHandle(ILineOfInvoiceRepository lineRepository)
        {
            _lineRepository = lineRepository;
        }

        public async Task<CreateLineOfInvoiceHandleResponse> Handle(CreateLineOfInvoiceHandleRequest request, CancellationToken cancellationToken)
        {
            // 🔍 Validasyonlar
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

            // 🧱 Entity oluşturma
            var line = new LineOfInvoice
            {
                InvoiceId = request.InvoiceId,
                ProductAndServiceId = request.ProductAndServiceId,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice,
                Status = DatabaseAccessLayer.Enumerations.Status.Active
            };

            // 💾 Veritabanına ekle
            _lineRepository.Add(line);

            // ✅ Başarılı dönüş
            return new CreateLineOfInvoiceHandleResponse
            {
                Message = "Fatura satırı başarıyla oluşturuldu.",
                Error = false
            };
        }
    }

}

