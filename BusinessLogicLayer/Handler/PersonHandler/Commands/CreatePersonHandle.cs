using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.PersonHandler.DTOs;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.PersonHandler.Commands
{
    public class CreatePersonHandle : IRequestHandler<CreatePersonHandleRequest, CreatePersonHandleResponse>
    {
        private readonly IPersonRepository _repository;

        //DI Constructor
        public CreatePersonHandle(IPersonRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreatePersonHandleResponse> Handle(CreatePersonHandleRequest request, CancellationToken cancellationToken)
        {
            string message = null;

            if (request == null)
                message = "Request Boş Olamaz";
            else if (string.IsNullOrWhiteSpace(request.Name))
                message = "İsim Zorunludur";
            else if (request.Name.Length > 100)
                message = "İsim 100 karakterden uzun olamaz.";
            else if (request.IdentityNumber <= 0 ||
                     (request.IdentityNumber.ToString().Trim().Length != 10 &&
                      request.IdentityNumber.ToString().Trim().Length != 11))
                message = "TCKN 11 hane, VKN 10 hane olmalıdır.";
            else if (!System.Text.RegularExpressions.Regex.IsMatch(request.Name, @"^[\p{L}0-9 ]{2,100}$"))
                message = "İsim yalnızca harf, rakam ve boşluk içermelidir (2-100).";
            else if (!string.IsNullOrWhiteSpace(request.TaxOffice) && request.TaxOffice.Length > 150)
                message = "Vergi Dairesi adı uzunluğu 150 karakterden fazla olamaz";
            else if (!Enum.IsDefined(typeof(PersonType), request.Type))
                message = "Kullanıcı Tipi Uyumlu Değildir.";
            else if (!Enum.IsDefined(typeof(Status), request.Status))
                message = "Durum bilgisi geçersiz.";

            if (message != null)
            {
                return new CreatePersonHandleResponse
                {
                    Message = message,
                    Error = true
                };
            }

            // Aynı IdentityNumber'a sahip kişi var mı? (Unique constraint benzeri)
            var exists = _repository.FirstOrDefault(p => p.IdentityNumber == request.IdentityNumber);
            if (exists != null)
            {
                return new CreatePersonHandleResponse
                {
                    Message = "Aynı TC/Vergi No ile kişi zaten mevcut.",
                    Error = true
                };
            }

            var person = new Person
            {
                Name = request.Name.Trim(),
                IdentityNumber = request.IdentityNumber,
                TaxOffice = string.IsNullOrWhiteSpace(request.TaxOffice) ? null : request.TaxOffice.Trim(),
                Type = request.Type,
                Status = request.Status
            };

            _repository.Add(person);

            return new CreatePersonHandleResponse
            {
                Message = "Kişi oluşturuldu",
                Error = false,
                Id = person.Id,
            };
        }
    }
}
