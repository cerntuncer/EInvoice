using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.AddressHandler.DTOs;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.AddressHandler.Commands
{
    public class CreateAddressHandle : IRequestHandler<CreateAddressHandleRequest, CreateAddressHandleResponse>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IPersonRepository _personRepository;

        public CreateAddressHandle(IAddressRepository addressRepository, IPersonRepository personRepository)
        {
            _addressRepository = addressRepository;
            _personRepository = personRepository;
        }

        public async Task<CreateAddressHandleResponse> Handle(CreateAddressHandleRequest request, CancellationToken cancellationToken)
        {
            string message = null;

            if (request == null)
                message = "Request boş olamaz.";

            else if (string.IsNullOrWhiteSpace(request.Text))
                message = "Adres metni boş olamaz.";

            else if (request.PersonId <= 0)
                message = "PersonId geçersiz.";

            else if (!Enum.IsDefined(typeof(AddressType), request.AddressType))
                message = "Geçersiz adres tipi.";

            else
            {
                var person = _personRepository.Find(request.PersonId);
                if (person == null)
                    message = "PersonId bulunamadı.";
                else if (person.Status != Status.Active)
                    message = "Bu kişiye adres atanamaz çünkü pasif durumdadır.";
            }

            if (message != null)
            {
                return new CreateAddressHandleResponse
                {
                    Message = message,
                    Error = true
                };
            }

            var address = new Address
            {
                AddressType = request.AddressType,
                Text = request.Text,
                PersonId = request.PersonId,
                Status = Status.Active
            };

            _addressRepository.Add(address);

            return new CreateAddressHandleResponse
            {
                Message = "Adres başarıyla oluşturuldu.",
                Error = false
            };
        }
    }
}
