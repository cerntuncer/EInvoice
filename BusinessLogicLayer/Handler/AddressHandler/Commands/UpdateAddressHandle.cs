using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.AddressHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.AddressHandler.Commands
{
    public class UpdateAddressHandle : IRequestHandler<UpdateAddressHandleRequest, UpdateAddressHandleResponse>
    {
        private readonly IAddressRepository _addressRepository;

        public UpdateAddressHandle(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<UpdateAddressHandleResponse> Handle(UpdateAddressHandleRequest request, CancellationToken cancellationToken)
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
                    message = "Bu adres belirtilen kişiye ait değil. Güncellenemez.";

                // --- Metin kontrolü ---
                if (message == null)
                {
                    var text = request.Text?.Trim();
                    if (string.IsNullOrWhiteSpace(text) || text!.Length > 500)
                        message = "Adres metni boş olamaz ve 500 karakteri geçemez.";
                }

                // --- Enum kontrolü ---
                if (message == null && !Enum.IsDefined(typeof(AddressType), request.AddressType))
                    message = "Adres tipi geçersiz.";
            }

            if (message != null)
            {
                return new UpdateAddressHandleResponse
                {
                    Error = true,
                    Message = message
                };
            }

            // --- Güncelleme ---
            address!.Text = request.Text!.Trim();
            address.AddressType = request.AddressType;

            _addressRepository.Update(address);

            return new UpdateAddressHandleResponse
            {
                Error = false,
                Message = "Adres başarıyla güncellendi."
            };
        }
    }
}
