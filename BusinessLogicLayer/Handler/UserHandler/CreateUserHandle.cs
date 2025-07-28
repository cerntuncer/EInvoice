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

        // DI constructor: concrete sınıf değil interface alıyoruz
        public CreateUserHandle(IUserRepository userRepository,
                                IPersonRepository personRepository)
        {
            _userRepository = userRepository;
            _personRepository = personRepository;
        }

        public async Task<CreateUserHandleResponse> Handle(CreateUserHandleRequest request,
                                                           CancellationToken cancellationToken)
        {
            string message = null;

            if (request == null)
                message = "Request boş olamaz.";

            else if (!Enum.IsDefined(typeof(UserType), request.Type))
                message = "Kullanıcı tipi geçersiz.";

            else if (request.PersonId <= 0)
                message = "PersonId geçersiz.";

            else
            {
                var person = _personRepository.Find(request.PersonId);

                if (person == null)
                    message = "PersonId bulunamadı.";
                else if (person.Status != Status.Active)
                    message = "Pasif kişiye kullanıcı atanamaz.";
            }

            if (message != null)
            {
                return new CreateUserHandleResponse
                {
                    Message = message,
                    Error = true
                };
            }

            var user = new User
            {
                Type = request.Type,
                PersonId = request.PersonId,
                Status = request.Status
            };

            _userRepository.Add(user);

            return new CreateUserHandleResponse
            {
                Message = "Kullanıcı oluşturuldu.",
                Error = false
            };
        }
    }
}
