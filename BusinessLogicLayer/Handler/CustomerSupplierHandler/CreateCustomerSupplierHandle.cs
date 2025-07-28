using BusinessLogicLayer.DesignPatterns.GenericRepositories.ConcRepositories;
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.PersonHandler;
using BusinessLogicLayer.Handler.UserHandler;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Handler.CustomerSupplierHandler
{
    public class CreateCustomerSupplierHandle : IRequestHandler<CreateCustomerSupplierHandleRequest, CreateCustomerSupplierHandleResponse>
    {
        // CustomerSupplier işlemleri için repository
        private readonly CustomerSupplierRepository _customerSupplierRepository;

        // Person (kişi) işlemleri için repository
        private readonly PersonRepository _personRepository;

       
        public CreateCustomerSupplierHandle()
        {
            _customerSupplierRepository = new CustomerSupplierRepository();
            _personRepository = new PersonRepository();
        }

        // Request'e göre müşteri/tedarikçi oluşturur
        public async Task<CreateCustomerSupplierHandleResponse> Handle(CreateCustomerSupplierHandleRequest request, CancellationToken cancellationToken)
        {
            string message = null;
            if (request == null)
                message = "Request boş olamaz.";

            // Müşteri veya Tedarikçi tipi geçersizse hata ver
            else if (!Enum.IsDefined(typeof(CustomerOrSupplierType), request.Type))
                message = "Müşteri veya Tedarikçi tipi geçersiz.";

            // Geçersiz PersonId verilmişse
            else if (request.PersonId <= 0)
                message = "Kişi Id geçersiz.";

            // Veritabanında ilgili PersonId bulunamazsa
            var person = _personRepository.Find(request.PersonId);
            if (person == null)
                message = "Kişi Id bulunamadı.";

            // hata oluşursa
            if (message != null)
            {
                return new CreateCustomerSupplierHandleResponse
                {
                    Message = message,
                    Error = true
                };
            }

            // Yeni müşteri veya tedarikçi nesnesi oluşturuluyor
            var customerSupplier = new CustomerSupplier
            {
                Type = request.Type,
                PersonId = request.PersonId,
                Status = Status.Active // varsayılan olarak aktif
            };

            // Veritabanına ekleniyor
            _customerSupplierRepository.Add(customerSupplier);

            // Başarılı yanıt dönülüyor
            return new CreateCustomerSupplierHandleResponse
            {
                Message = "Müşteri/Tedarikçi başarıyla oluşturuldu.",
                Error = false
            };
        }
    }
}
