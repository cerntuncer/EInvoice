using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.BankHandler;
using BusinessLogicLayer.Handler.UserHandler.DTOs;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.UserHandler.Commands
{
    public class CreateUserHandle : IRequestHandler<CreateUserHandleRequest, CreateUserHandleResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonRepository _personRepository;
        private readonly ICustomerSupplierRepository _customerSupplierRepository;
        private readonly IMediator _mediator;

        public CreateUserHandle(IUserRepository userRepository, IPersonRepository personRepository, ICustomerSupplierRepository customerSupplierRepository, IMediator mediator)
        {
            _userRepository = userRepository;
            _personRepository = personRepository;
            _customerSupplierRepository = customerSupplierRepository;
            _mediator = mediator;
        }

        public async Task<CreateUserHandleResponse> Handle(CreateUserHandleRequest request, CancellationToken cancellationToken)
        {
            string message = null;
            long? personId = null;
            bool person = true;
            if (!Enum.IsDefined(typeof(Status), request.Status))
                message = "Durum bilgisi geçersiz.";
            else if (!Enum.IsDefined(typeof(Type), request.Type))
                message = "Kullanıcı tipi geçersiz.";
            else if (request.PersonId == null)
            {
                if (request.Person == null)
                    message = "PersonId ya da Yeni oluşturalacak Person Bilgileri iletilmelidir";
                else
                {
                    var newPerson = await _mediator.Send(request.Person, cancellationToken);
                    if (newPerson.Error == false)
                    {
                        personId = newPerson.Id.Value;
                        person = false;
                    }
                    else
                    {
                        message = newPerson.Message;
                    }
                }
            }
            else if (_personRepository.Find(request.PersonId.Value) == null)
            {
                message = "Gönderilen Id ye uygun Kişi bulunamadı";
            }
            if (person) {
                var existingUser = _userRepository.FirstOrDefault(b => b.PersonId == request.PersonId);
                var existingCustomerSupplier = _customerSupplierRepository.FirstOrDefault(b => b.PersonId == request.PersonId);
                if (existingUser != null || existingCustomerSupplier != null)
                {
                    message = "Belirtilen Kullanıcı Başka Bir Kullanıcı, Tedarikçi ya da Müşteriye Bağlı";
                }
            }
          
            if (message != null)
            {
                return new CreateUserHandleResponse
                {
                    Message = message,
                    Error = true
                };
            }

            if (personId == null)
            {
                personId = request.PersonId.Value;
            }
            // 👤 User oluştur
            var user = new User
            {
                Type = request.Type,
                PersonId = personId.Value,
                Status = request.Status
            };
            _userRepository.Add(user);

            if (person)
            {
                message = "Kullanıcı başarıyla oluşturuldu.";
            }
            else
            {
                message = "Kullanıcı ve kişi başarıyla oluşturuldu.";
            }
            return new CreateUserHandleResponse
            {
                Message = message,
                Error = false
            };
        }
    }
}
