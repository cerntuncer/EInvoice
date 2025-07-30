using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.CustomerSupplierHandler
{
    public class CreateCustomerSupplierHandle : IRequestHandler<CreateCustomerSupplierHandleRequest, CreateCustomerSupplierHandleResponse>
    {
        private readonly ICustomerSupplierRepository _customerSupplierRepository;
        private readonly IPersonRepository _personRepository;

        public CreateCustomerSupplierHandle(ICustomerSupplierRepository customerSupplierRepository, IPersonRepository personRepository)
        {
            _customerSupplierRepository = customerSupplierRepository;
            _personRepository = personRepository;
        }

        public async Task<CreateCustomerSupplierHandleResponse> Handle(CreateCustomerSupplierHandleRequest request, CancellationToken cancellationToken)
        {
            string message = null;

            if (string.IsNullOrWhiteSpace(request.Name))
                message = "İsim boş olamaz.";
            else if (request.IdentityNumber <= 0 || request.IdentityNumber.ToString().Length > 11)
                message = "TCKN geçersiz.";
            else if (!Enum.IsDefined(typeof(CustomerOrSupplierType), request.Type))
                message = "Müşteri veya Tedarikçi tipi geçersiz.";


            if (message != null)
            {
                return new CreateCustomerSupplierHandleResponse
                {
                    Message = message,
                    Error = true
                };
            }

            // Person oluştur
            var person = new Person
            {
                Name = request.Name,
                IdentityNumber = request.IdentityNumber,
                TaxOffice = request.TaxOffice,
                Type = PersonType.CustomerOrSupplier, 
                Status = Status.Active
            };

            _personRepository.Add(person);

            // Müşteri/Tedarikçi oluştur
            var customerSupplier = new CustomerSupplier
            {
                Type = request.Type,
                PersonId = person.Id,
                Status = Status.Active
            };
            _customerSupplierRepository.Add(customerSupplier);

            return new CreateCustomerSupplierHandleResponse
            {
                Message = "Müşteri/Tedarikçi ve kişi oluşturuldu.",
                Error = false
            };
        }
    }
}
