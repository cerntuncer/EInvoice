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
            if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 100)
            {
                return new CreateProductAndServiceHandleResponse
                {
                    Message = "Ürün/hizmet adı boş olamaz ve 100 karakteri geçemez.",
                    Error = true
                };
            }

            if (request.UnitPrice < 0)
            {
                return new CreateProductAndServiceHandleResponse
                {
                    Message = "Birim fiyat negatif olamaz.",
                    Error = true
                };
            }

            if (!Enum.IsDefined(typeof(UnitType), request.UnitType))
            {
                return new CreateProductAndServiceHandleResponse
                {
                    Message = "Geçersiz birim türü.",
                    Error = true
                };
            }

            var entity = new ProductAndService
            {
                Name = request.Name,
                price = request.UnitPrice,
                UnitType = request.UnitType,
                Status = Status.Active
            };

            _repository.Add(entity);

            return new CreateProductAndServiceHandleResponse
            {
                Message = "Ürün/hizmet başarıyla oluşturuldu.",
                Error = false
            };
        }
    }
}
