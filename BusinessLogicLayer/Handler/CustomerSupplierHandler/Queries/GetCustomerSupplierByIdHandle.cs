using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.CustomerSupplierHandler.DTOs;
using MediatR;

namespace BusinessLogicLayer.Handler.CustomerSupplierHandler.Queries
{
    public class GetCustomerSupplierByIdHandle : IRequestHandler<GetCustomerSupplierByIdHandleRequest, GetCustomerSupplierByIdHandleResponse>
    {
        private readonly ICustomerSupplierRepository _customerSupplierRepository;

        public GetCustomerSupplierByIdHandle(ICustomerSupplierRepository customerSupplierRepository)
        {
            _customerSupplierRepository = customerSupplierRepository;
        }

        public async Task<GetCustomerSupplierByIdHandleResponse> Handle(GetCustomerSupplierByIdHandleRequest request, CancellationToken cancellationToken)
        {
            var entity =  _customerSupplierRepository.FirstOrDefault(x => x.Id == request.Id && x.UserId == request.UserId);

            if (entity == null)
            {
                return new GetCustomerSupplierByIdHandleResponse
                {
                    Error = true,
                    Message = "Müşteri/Tedarikçi bulunamadı."
                };
            }

            return new GetCustomerSupplierByIdHandleResponse
            {
                Error = false,
                Message = "Müşteri/Tedarikçi getirildi.",
                Id = entity.Id,
                Type = entity.Type,
                PersonId = entity.PersonId,
                Status = entity.Status
            };
        }
    }
}
