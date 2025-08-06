using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using MediatR;

namespace BusinessLogicLayer.Handler.ProductAndServiceHandler
{
    public class GetProductAndServiceByIdHandle : IRequestHandler<GetProductAndServiceByIdHandleRequest, GetProductAndServiceByIdHandleResponse>
    {
        private readonly IProductAndServiceRepository _repository;

        public GetProductAndServiceByIdHandle(IProductAndServiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetProductAndServiceByIdHandleResponse> Handle(GetProductAndServiceByIdHandleRequest request, CancellationToken cancellationToken)
        {
            var entity = _repository.GetById(request.Id);

            if (entity == null)
            {
                return new GetProductAndServiceByIdHandleResponse
                {
                    Error = true,
                    Message = "Ürün veya hizmet bulunamadı."
                };
            }

            return new GetProductAndServiceByIdHandleResponse
            {
                Error = false,
                Message = "Ürün/hizmet başarıyla getirildi.",
                Id = entity.Id,
                Name = entity.Name,
                UnitPrice = entity.price,
                UnitType = entity.UnitType,
                Status = entity.Status
            };
        }
    }
}
