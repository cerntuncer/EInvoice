using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using MediatR;

namespace BusinessLogicLayer.Handler.UserHandler
{
    public class UpdateUserHandle : IRequestHandler<UpdateUserHandleRequest, UpdateUserHandleResponse>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserHandle(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UpdateUserHandleResponse> Handle(UpdateUserHandleRequest request, CancellationToken cancellationToken)
        {
            var user = _userRepository.Find(request.Id);

            if (user == null)
                return new UpdateUserHandleResponse { Error = true, Message = "Kullanıcı bulunamadı." };

            user.Type = request.Type;
            user.Status = request.Status;

            _userRepository.Update(user);

            return new UpdateUserHandleResponse { Error = false, Message = "Kullanıcı güncellendi." };
        }
    }
}
