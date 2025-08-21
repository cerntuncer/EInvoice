using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.ProductAndServiceHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.ProductAndServiceHandler.Queries
{
    public class GetProductsAndServicesByUserIdHandle : IRequestHandler<GetProductsAndServicesByUserIdHandleRequest, GetProductsAndServicesByUserIdHandleResponse>
    {
        private readonly IProductAndServiceRepository _repository;

        public GetProductsAndServicesByUserIdHandle(IProductAndServiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetProductsAndServicesByUserIdHandleResponse> Handle(GetProductsAndServicesByUserIdHandleRequest request, CancellationToken cancellationToken)
        {
            var items = _repository
                .Where(p => p.UserId == request.UserId && p.Status != Status.Passive)
                .OrderBy(p => p.Name)
                .ToList();

            if (!items.Any())
            {
                return new GetProductsAndServicesByUserIdHandleResponse
                {
                    Error = true,
                    Message = "Bu kullanıcıya ait ürün/hizmet bulunamadı."
                };
            }

            return new GetProductsAndServicesByUserIdHandleResponse
            {
                Error = false,
                Message = "Ürün/hizmetler başarıyla getirildi.",
                Items = items.Select(x => new ProductAndServiceListItemDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UnitPrice = x.price,
                    UnitType = x.UnitType,
                    Status = x.Status
                }).ToList()
            };
        }
    }
}