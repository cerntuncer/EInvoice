using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.UserHandler.DTOs;
using MediatR;

namespace BusinessLogicLayer.Handler.UserHandler.Queries
{
    public class GetUserWithPersonByIdHandle : IRequestHandler<GetUserWithPersonByIdHandleRequest, GetUserWithPersonByIdHandleResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonRepository _personRepository;

        public GetUserWithPersonByIdHandle(IUserRepository userRepository, IPersonRepository personRepository)
        {
            _userRepository = userRepository;
            _personRepository = personRepository;
        }

        public async Task<GetUserWithPersonByIdHandleResponse> Handle(GetUserWithPersonByIdHandleRequest request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetById(request.Id);
            if (user == null)
            {
                return new GetUserWithPersonByIdHandleResponse
                {
                    Error = true,
                    Message = "Kullanıcı bulunamadı."
                };
            }

            var person = _personRepository.GetById(user.PersonId);
            if (person == null)
            {
                return new GetUserWithPersonByIdHandleResponse
                {
                    Error = true,
                    Message = "İlişkili kişi bulunamadı."
                };
            }

            return new GetUserWithPersonByIdHandleResponse
            {
                Error = false,
                Message = "Kullanıcı ve kişi bilgileri başarıyla getirildi.",
                UserId = user.Id,
                UserType = user.Type,
                UserStatus = user.Status,
                PersonId = user.PersonId,
                PersonName = person.Name,
                IdentityNumber = person.IdentityNumber,
                TaxOffice = person.TaxOffice,
                PersonType = person.Type,
                PersonStatus = person.Status
            };
        }
    }
}