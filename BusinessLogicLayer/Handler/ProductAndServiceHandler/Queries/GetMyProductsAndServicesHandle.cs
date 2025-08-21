using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.ProductAndServiceHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.ProductAndServiceHandler.Queries
{
    public class GetMyProductsAndServicesHandleRequest : IRequest<GetMyProductsAndServicesHandleResponse>
    {
        public long UserId { get; set; }
    }

    public class GetMyProductsAndServicesHandleResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public List<MyProductAndServiceListItemDto> Items { get; set; } = new();
    }

    public class MyProductAndServiceListItemDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public UnitType UnitType { get; set; }
        public Status Status { get; set; }
    }

    public class GetMyProductsAndServicesHandle : IRequestHandler<GetMyProductsAndServicesHandleRequest, GetMyProductsAndServicesHandleResponse>
    {
        private readonly IProductAndServiceRepository _repository;

        public GetMyProductsAndServicesHandle(IProductAndServiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetMyProductsAndServicesHandleResponse> Handle(GetMyProductsAndServicesHandleRequest request, CancellationToken cancellationToken)
        {
            if (request.UserId <= 0)
            {
                return new GetMyProductsAndServicesHandleResponse
                {
                    Error = true,
                    Message = "Geçersiz kullanıcı bilgisi."
                };
            }

            var list = _repository.Where(x => x.UserId == request.UserId && x.Status == Status.Active)
                                   .Select(x => new MyProductAndServiceListItemDto
                                   {
                                       Id = x.Id,
                                       Name = x.Name,
                                       UnitPrice = x.price,
                                       UnitType = x.UnitType,
                                       Status = x.Status
                                   })
                                   .ToList();

            return new GetMyProductsAndServicesHandleResponse
            {
                Error = false,
                Message = "Kayıtlar getirildi.",
                Items = list
            };
        }
    }
}