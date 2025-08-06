using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.AddressHandler
{
    public class GetAddressesByPersonIdHandle : IRequestHandler<GetAddressesByPersonIdRequest, GetAddressesByPersonIdResponse>
    {
        private readonly IAddressRepository _addressRepository;

        public GetAddressesByPersonIdHandle(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<GetAddressesByPersonIdResponse> Handle(GetAddressesByPersonIdRequest request, CancellationToken cancellationToken)
        {
            if (request.PersonId <= 0)
            {
                return new GetAddressesByPersonIdResponse
                {
                    Error = true,
                    Message = "Geçersiz PersonId"
                };
            }

            var addresses = _addressRepository.Where(x => x.PersonId == request.PersonId && x.Status != Status.Passive).ToList();

            if (addresses == null || !addresses.Any())
            {
                return new GetAddressesByPersonIdResponse
                {
                    Error = true,
                    Message = "Bu kişiye ait adres bulunamadı."
                };
            }

            return new GetAddressesByPersonIdResponse
            {
                Error = false,
                Message = "Adresler getirildi.",
                Addresses = addresses.Select(a => new AddressDto
                {
                    Id = a.Id,
                    Text = a.Text,
                    AddressType = a.AddressType,
                    Status = a.Status
                }).ToList()
            };
        }
    }

}
