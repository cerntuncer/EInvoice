using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.UserHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.UserHandler.Queries
{
    public class GetUsersWithPersonListHandle : IRequestHandler<GetUsersWithPersonListHandleRequest, GetUsersWithPersonListHandleResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonRepository _personRepository;

        public GetUsersWithPersonListHandle(IUserRepository userRepository, IPersonRepository personRepository)
        {
            _userRepository = userRepository;
            _personRepository = personRepository;
        }

        public async Task<GetUsersWithPersonListHandleResponse> Handle(GetUsersWithPersonListHandleRequest request, CancellationToken cancellationToken)
        {
            var users = _userRepository.GetAll();

            var response = new GetUsersWithPersonListHandleResponse
            {
                Error = false,
                Message = "Kullanıcı listesi getirildi",
                Users = new List<UserWithPersonListItemDto>()
            };

            foreach (var user in users)
            {
                var person = _personRepository.GetById(user.PersonId);
                var item = new UserWithPersonListItemDto
                {
                    UserId = user.Id,
                    UserType = user.Type,
                    PersonId = user.PersonId,
                    PersonName = person?.Name ?? string.Empty,
                    PersonIdentityNumber = person.IdentityNumber,
                    PersonTaxOffice = person.TaxOffice,
                };
                response.Users.Add(item);
            }

            return response;
        }
    }
}
