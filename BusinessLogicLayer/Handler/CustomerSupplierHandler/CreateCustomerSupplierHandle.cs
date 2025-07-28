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

            if (request == null)
                message = "Request boş olamaz.";

            else if (!Enum.IsDefined(typeof(CustomerOrSupplierType), request.Type))
                message = "Müşteri veya Tedarikçi tipi geçersiz.";

            else if (request.PersonId <= 0)
                message = "Kişi Id geçersiz.";

            var person = _personRepository.Find(request.PersonId);
            if (person == null)
                message = "Kişi Id bulunamadı.";

            if (message != null)
            {
                return new CreateCustomerSupplierHandleResponse
                {
                    Message = message,
                    Error = true
                };
            }

            var customerSupplier = new CustomerSupplier
            {
                Type = request.Type,
                PersonId = request.PersonId,
                Status = Status.Active
            };

            _customerSupplierRepository.Add(customerSupplier);

            return new CreateCustomerSupplierHandleResponse
            {
                Message = "Müşteri/Tedarikçi başarıyla oluşturuldu.",
                Error = false
            };
        }
    }
}
