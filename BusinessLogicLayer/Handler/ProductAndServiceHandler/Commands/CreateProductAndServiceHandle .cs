using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.ProductAndServiceHandler.DTOs;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.ProductAndServiceHandler.Commands
{
    public class CreateProductAndServiceHandle : IRequestHandler<CreateProductAndServiceHandleRequest, CreateProductAndServiceHandleResponse>
    {
        private readonly IProductAndServiceRepository _repository;

        public CreateProductAndServiceHandle(IProductAndServiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreateProductAndServiceHandleResponse> Handle(CreateProductAndServiceHandleRequest request, CancellationToken cancellationToken)
        {
            string? message = null;

            var name = request.Name?.Trim();
            if (string.IsNullOrWhiteSpace(name) || name!.Length > 100)
                message = "Ürün/hizmet adı boş olamaz ve 100 karakteri geçemez.";
            else if (request.UnitPrice < 0)
                message = "Birim fiyat negatif olamaz.";
            else if (!Enum.IsDefined(typeof(UnitType), request.UnitType))
                message = "Geçersiz birim türü.";
            else if (request.UserId <= 0)
                message = "Geçersiz kullanıcı bilgisi.";

            if (message != null)
            {
                return new CreateProductAndServiceHandleResponse
                {
                    Error = true,
                    Message = message
                };
            }

            var entity = new ProductAndService
            {
                Name = name!,
                price = request.UnitPrice,
                UnitType = request.UnitType,
                Status = Status.Active,
                UserId = request.UserId
            };

            _repository.Add(entity);

            return new CreateProductAndServiceHandleResponse
            {
                Error = false,
                Message = "Ürün/hizmet başarıyla oluşturuldu."
            };
        }
    }
}
