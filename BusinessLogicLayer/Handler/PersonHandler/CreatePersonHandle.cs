using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;


namespace BusinessLogicLayer.Handler.PersonHandler
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
            else if (request.Name.Length > 50)
                message = "İsim 50 karakterden uzun olamaz.";
            else if (request.IdentityNumber <= 0 || request.IdentityNumber.ToString().Length > 11)
                message = "TCKN veya VKN uzunluğu hatalıdır.";
            else if (!string.IsNullOrWhiteSpace(request.TaxOffice) && request.TaxOffice.Length > 150)
                message = "Vergi Dairesi adı uzunluğu 150 karakterden fazla olamaz";
            else if (!Enum.IsDefined(typeof(PersonType), request.Type))
                message = "Kullanıcı Tipi Uyumlu Değildir.";

            if (message != null)
            {
                return new CreatePersonHandleResponse
                {
                    Message = message,
                    Error = true
                };
            }

            var person = new Person
            {
                Name = request.Name,
                IdentityNumber = request.IdentityNumber,
                TaxOffice = request.TaxOffice,
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
