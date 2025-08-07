using BusinessLogicLayer.DesignPatterns.GenericRepositories.ConcRepositories;
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.CustomerSupplierHandler.DTOs;
using BusinessLogicLayer.Handler.UserHandler;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.CustomerSupplierHandler.Commands
{
    public class CreateCustomerSupplierHandle : IRequestHandler<CreateCustomerSupplierHandleRequest, CreateCustomerSupplierHandleResponse>
    {
        private readonly ICustomerSupplierRepository _customerSupplierRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IMediator _mediator;

        public CreateCustomerSupplierHandle(IUserRepository userRepository, IPersonRepository personRepository, ICustomerSupplierRepository customerSupplierRepository, IMediator mediator)
        {
            _userRepository = userRepository;
            _personRepository = personRepository;
            _customerSupplierRepository = customerSupplierRepository;
            _mediator = mediator;
        }
        public async Task<CreateCustomerSupplierHandleResponse> Handle(CreateCustomerSupplierHandleRequest request, CancellationToken cancellationToken)
        {
            string message = null;
            long? personId = null;
            bool person = true;
            if (!Enum.IsDefined(typeof(CustomerOrSupplierType), request.Type))
                message = "Müşteri/Tedarikçi tipi geçersiz.";
            else if (!Enum.IsDefined(typeof(Status), request.Status))
                message = "Durum bilgisi geçersiz.";
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
            if (person)
            {
                var existingUser = _userRepository.FirstOrDefault(b => b.PersonId == request.PersonId);
                var existingCustomerSupplier = _customerSupplierRepository.FirstOrDefault(b => b.PersonId == request.PersonId);
                if (existingUser != null || existingCustomerSupplier != null)
                {
                    message = "Belirtilen Kullanıcı Başka Bir Kullanıcı, Tedarikçi ya da Müşteriye Bağlı";
                }
            }
            if (message != null)
            {
                return new CreateCustomerSupplierHandleResponse
                {
                    Message = message,
                    Error = true
                };
            }

            if (personId == null)
            {
                personId = request.PersonId.Value;
            }
            var customerSupplier = new CustomerSupplier
            {
                Type = request.Type,
                PersonId = personId.Value,
                Status = request.Status
            };
            _customerSupplierRepository.Add(customerSupplier);
            if (person)
            {
                message = "Müşteri/Tedarikçi başarıyla oluşturuldu.";
            }
            else
            {
                message = "Müşteri/Tedarikçi ve kişi başarıyla oluşturuldu.";
            }
            return new CreateCustomerSupplierHandleResponse
            {
                Message = message,
                Error = false
            };
        }
    }
}
