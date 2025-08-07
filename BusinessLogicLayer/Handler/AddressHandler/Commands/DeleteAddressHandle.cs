using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.AddressHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.AddressHandler.Commands
{
    public class DeleteAddressHandle : IRequestHandler<DeleteAddressHandleRequest, DeleteAddressHandleResponse>
    {
        private readonly IAddressRepository _addressRepository;

        public DeleteAddressHandle(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<DeleteAddressHandleResponse> Handle(DeleteAddressHandleRequest request, CancellationToken cancellationToken)
        {
            var address = _addressRepository.Find(request.Id);

            if (address == null)
            {
                return new DeleteAddressHandleResponse
                {
                    Error = true,
                    Message = "Adres bulunamadı."
                };
            }

            if (address.PersonId != request.PersonId)
            {
                return new DeleteAddressHandleResponse
                {
                    Error = true,
                    Message = "Bu adres belirtilen kişiye ait değil. Silinemez."
                };
            }

            address.Status = Status.Passive;
            _addressRepository.Update(address); // Soft delete

            return new DeleteAddressHandleResponse
            {
                Error = false,
                Message = "Adres başarıyla silindi (pasif hale getirildi)."
            };
        }
    }
}
