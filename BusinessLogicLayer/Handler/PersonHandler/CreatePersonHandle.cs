using BusinessLogicLayer.DesignPatterns.GenericRepositories.ConcRepositories;
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BusinessLogicLayer.Handler.PersonHandler
{
    public class CreatePersonHandle : IRequestHandler<CreatePersonHandleRequest,CreatePersonHandleResponse>
    {
        private readonly PersonRepository _repository;

        public CreatePersonHandle() { 
            _repository = new PersonRepository();
        }
        async Task<CreatePersonHandleResponse> IRequestHandler<CreatePersonHandleRequest, CreatePersonHandleResponse>.Handle(CreatePersonHandleRequest request, CancellationToken cancellationToken)
        {
            string message = null;
            if (request == null)
                message = "Request Boş Olamaz";

            // Name kontrolü
            else if (string.IsNullOrWhiteSpace(request.Name))
                message = "İsim Zorunludur";

            else if (request.Name.Length > 50)
                message = "İsim 50 karakterden uzun olamaz.";

            // IdentityNumber kontrolü (örneğin: 11 haneli TC veya VKN gibi)
            else if (request.IdentityNumber <= 0 || request.IdentityNumber.ToString().Length > 11)
                message = "TCKN veya VKN uzunluğu hatalıdır.";

            // TaxOffice kontrolü (opsiyonel ama varsa uzunluk önemli)
            else if (!string.IsNullOrWhiteSpace(request.TaxOffice) && request.TaxOffice.Length > 150)
                message = "Vergi Dairesi adı uzunluğu 150 karakterden fazla olamaz";

            // PersonType kontrolü (enum değerinin geçerli olup olmadığını kontrol edebilirsin)
            else if (!Enum.IsDefined(typeof(PersonType), request.Type))
                message = "Kullanıcı Tipi Uyumlu Değildir.";
            
            if(message != null)
            {
                return new CreatePersonHandleResponse
                {
                    Message = message,
                    Error = true
                };
            }
            // Kişi oluşturma
            var person = new Person
            {
                Name = request.Name,
                IdentityNumber = request.IdentityNumber,
                TaxOffice = request.TaxOffice,
                Type = request.Type,
            };

             _repository.Add(person);

            return new CreatePersonHandleResponse
            {
                Message = "Kişi oluşturuldu",
                Error = false
            };
        }

    }
}
