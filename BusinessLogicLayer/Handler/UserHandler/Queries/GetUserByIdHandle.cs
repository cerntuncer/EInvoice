using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.UserHandler.DTOs;
using MediatR;

namespace BusinessLogicLayer.Handler.UserHandler.Queries
{
    public class GetUserByIdHandle : IRequestHandler<GetUserByIdHandleRequest, GetUserByIdHandleResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdHandle(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserByIdHandleResponse> Handle(GetUserByIdHandleRequest request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetById(request.Id);

            if (user == null)
            {
                return new GetUserByIdHandleResponse
                {
                    Error = true,
                    Message = "Kullanıcı bulunamadı."
                };
            }

            return new GetUserByIdHandleResponse
            {
                Error = false,
                Message = "Kullanıcı başarıyla getirildi.",
                Id = user.Id,
                Type = user.Type,
                PersonId = user.PersonId,
                Status = user.Status
            };
        }
    }
}
