using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.AddressHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.AddressHandler.Queries
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
            string? message = null;
            List<AddressDto>? dtos = null;

            // --- Validasyon ---
            if (request.PersonId <= 0)
            {
                message = "Geçersiz PersonId";
            }
            else
            {
                // --- Sorgu ---
                var addresses = _addressRepository
                    .Where(x => x.PersonId == request.PersonId && x.Status != Status.Passive)
                    .ToList();

                if (addresses.Count == 0)
                {
                    message = "Bu kişiye ait adres bulunamadı.";
                }
                else
                {
                    dtos = addresses.Select(a => new AddressDto
                    {
                        Id = a.Id,
                        Text = a.Text,
                        AddressType = a.AddressType,
                        Status = a.Status
                    }).ToList();
                }
            }

            if (message != null)
            {
                return new GetAddressesByPersonIdResponse
                {
                    Error = true,
                    Message = message
                };
            }

            return new GetAddressesByPersonIdResponse
            {
                Error = false,
                Message = "Adresler getirildi.",
                Addresses = dtos
            };
        }
    }
}
