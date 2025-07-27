using BusinessLogicLayer.DesignPatterns.GenericRepositories.ConcRepositories;
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.PersonHandler;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Handler.UserHandler
{
    public class CreateUserHandle : IRequestHandler<CreateUserHandleRequest, CreateUserHandleResponse>
    {
        private readonly UserRepository _userRepository;
        private readonly PersonRepository _personRepository;

        public CreateUserHandle()
        {
            _userRepository = new UserRepository();
            _personRepository = new PersonRepository();
        }
        async Task<CreateUserHandleResponse> IRequestHandler<CreateUserHandleRequest, CreateUserHandleResponse>.Handle(CreateUserHandleRequest request, CancellationToken cancellationToken)
        {
            string message = null;
            if (request == null)
                message = "Request Boş Olamaz";

            // PersonType kontrolü (enum değerinin geçerli olup olmadığını kontrol edebilirsin)
            else if (!Enum.IsDefined(typeof(UserType), request.Type))
                message = "Kullanıcı Tipi Uyumlu Değildir.";
            else if(request.PersonId == null)
            {
                message = "Kişi Id Null Olamaz.";
            }
            var data = _personRepository.Find(request.PersonId);
            if(data == null)
            {
                message = "Kişi Id Bulunamadı.";
            }

            if (message != null)
            {
                return new CreateUserHandleResponse
                {
                    Message = message,
                    Error = true
                };
            }
            // Kişi oluşturma
            var user = new User
            {
                Type = request.Type,
                PersonId = request.PersonId,
                Status = request.Status,
            };

            _userRepository.Add(user);

            return new CreateUserHandleResponse
            {
                Message = "Kullanıcı Oluşturuldu",
                Error = false
            };
        }
    }
}
