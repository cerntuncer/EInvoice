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
            var address = _addressRepository.Find(request.Id);
            if (address == null)
            {
                return new UpdateAddressHandleResponse
                {
                    Error = true,
                    Message = "Adres bulunamadı."
                };
            }

            // ✅ Kişi bu adresin sahibi mi kontrolü
            if (address.PersonId != request.PersonId)
            {
                return new UpdateAddressHandleResponse
                {
                    Error = true,
                    Message = "Bu adres belirtilen kişiye ait değil. Güncellenemez."
                };
            }

            if (string.IsNullOrWhiteSpace(request.Text) || request.Text.Length > 500)
            {
                return new UpdateAddressHandleResponse
                {
                    Error = true,
                    Message = "Adres metni boş olamaz ve 500 karakteri geçemez."
                };
            }

            if (!Enum.IsDefined(typeof(AddressType), request.AddressType))
            {
                return new UpdateAddressHandleResponse
                {
                    Error = true,
                    Message = "Adres tipi geçersiz."
                };
            }

            address.Text = request.Text;
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
