using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.AddressHandler.DTOs;
using MediatR;

namespace BusinessLogicLayer.Handler.AddressHandler.Queries
{
    public class GetAddressByIdHandle : IRequestHandler<GetAddressByIdHandleRequest, GetAddressByIdHandleResponse>
    {
        private readonly IAddressRepository _addressRepository;

        public GetAddressByIdHandle(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<GetAddressByIdHandleResponse> Handle(GetAddressByIdHandleRequest request, CancellationToken cancellationToken)
        {
            string message = null;

            if (request.Id <= 0)
                message = "Geçersiz ID.";

            var address =  _addressRepository.GetById(request.Id);
            if (address == null)
                message = "Adres bulunamadı.";

            if (message != null)
            {
                return new GetAddressByIdHandleResponse
                {
                    Message = message,
                    Error = true
                };
            }

            return new GetAddressByIdHandleResponse
            {
                Message = "Adres başarıyla getirildi.",
                Error = false,
                Id = address.Id,
                Text = address.Text,
                AddressType = address.AddressType,
                PersonId = address.PersonId,
                Status = address.Status
            };
        }
    }

}
