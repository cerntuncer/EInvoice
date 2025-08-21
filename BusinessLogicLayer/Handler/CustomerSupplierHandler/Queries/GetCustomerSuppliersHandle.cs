using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.CustomerSupplierHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;
using System.Linq;

namespace BusinessLogicLayer.Handler.CustomerSupplierHandler.Queries
{
    public class GetCustomerSuppliersHandle : IRequestHandler<GetCustomerSuppliersHandleRequest, GetCustomerSuppliersHandleResponse>
    {
        private readonly ICustomerSupplierRepository _repository;

        public GetCustomerSuppliersHandle(ICustomerSupplierRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetCustomerSuppliersHandleResponse> Handle(GetCustomerSuppliersHandleRequest request, CancellationToken cancellationToken)
        {
            var projected = _repository
                .Select(cs => new CustomerSupplierListItemDto
                {
                    Id = cs.Id,
                    PersonId = cs.PersonId,
                    PersonName = cs.Person.Name,
                    Type = cs.Type,
                    Status = cs.Status
                })
                .OrderBy(x => x.PersonName)
                .ToList();

            if (!projected.Any())
            {
                return new GetCustomerSuppliersHandleResponse
                {
                    Error = true,
                    Message = "Kayıt bulunamadı."
                };
            }

            return new GetCustomerSuppliersHandleResponse
            {
                Error = false,
                Message = "Kayıtlar başarıyla getirildi.",
                Items = projected
            };
        }
    }
}

