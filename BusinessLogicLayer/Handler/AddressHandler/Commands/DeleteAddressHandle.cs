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
            string? message = null;

            // --- Varlık kontrolü ---
            var address = _addressRepository.Find(request.Id);
            if (address == null)
            {
                message = "Adres bulunamadı.";
            }
            else
            {
                // --- Sahiplik kontrolü ---
                if (address.PersonId != request.PersonId)
                    message = "Bu adres belirtilen kişiye ait değil. Silinemez.";

                // (opsiyonel) Zaten pasif mi?
                if (message == null && address.Status == Status.Passive)
                    message = "Adres zaten pasif durumda.";
            }

            if (message != null)
            {
                return new DeleteAddressHandleResponse
                {
                    Error = true,
                    Message = message
                };
            }

            // --- Soft delete ---
            address!.Status = Status.Passive;
            _addressRepository.Update(address);

            return new DeleteAddressHandleResponse
            {
                Error = false,
                Message = "Adres başarıyla silindi (pasif hale getirildi)."
            };
        }
    }
}
