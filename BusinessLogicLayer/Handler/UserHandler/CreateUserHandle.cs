using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.UserHandler
{
    public class CreateUserHandle : IRequestHandler<CreateUserHandleRequest, CreateUserHandleResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonRepository _personRepository;

        public CreateUserHandle(IUserRepository userRepository, IPersonRepository personRepository)
        {
            _userRepository = userRepository;
            _personRepository = personRepository;
        }

        public async Task<CreateUserHandleResponse> Handle(CreateUserHandleRequest request, CancellationToken cancellationToken)
        {
            string message = null;

            if (string.IsNullOrWhiteSpace(request.Name))
                message = "İsim zorunludur.";
            else if (request.IdentityNumber <= 0 || request.IdentityNumber.ToString().Length > 11)
                message = "TCKN geçersiz.";
            else if (!Enum.IsDefined(typeof(UserType), request.Type))
                message = "Kullanıcı tipi geçersiz.";
            else if (!string.IsNullOrWhiteSpace(request.TaxOffice) && request.TaxOffice.Length > 150)
                message = "Vergi dairesi adı 150 karakteri geçemez.";
            else if (!Enum.IsDefined(typeof(Status), request.Status))
                message = "Durum bilgisi geçersiz.";

            if (message != null)
            {
                return new CreateUserHandleResponse
                {
                    Message = message,
                    Error = true
                };
            }

            // 🔄 Person oluştur
            var person = new Person
            {
                Name = request.Name,
                IdentityNumber = request.IdentityNumber,
                TaxOffice = request.TaxOffice,
                Type = PersonType.User,
                Status = Status.Active
            };
            _personRepository.Add(person);

            // 👤 User oluştur
            var user = new User
            {
                Type = request.Type,
                PersonId = person.Id,
                Status = request.Status
            };
            _userRepository.Add(user);

            return new CreateUserHandleResponse
            {
                Message = "Kullanıcı ve kişi başarıyla oluşturuldu.",
                Error = false
            };
        }
    }
}
