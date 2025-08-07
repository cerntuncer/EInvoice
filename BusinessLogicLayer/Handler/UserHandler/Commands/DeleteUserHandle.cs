using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.UserHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.UserHandler.Commands
{
    public class DeleteUserHandle : IRequestHandler<DeleteUserHandleRequest, DeleteUserHandleResponse>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserHandle(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<DeleteUserHandleResponse> Handle(DeleteUserHandleRequest request, CancellationToken cancellationToken)
        {
            var user = _userRepository.Find(request.Id);
            if (user == null)
                return new DeleteUserHandleResponse { Error = true, Message = "Kullanıcı bulunamadı." };

            user.Status = Status.Passive;
            _userRepository.Update(user);

            return new DeleteUserHandleResponse { Error = false, Message = "Kullanıcı silindi (pasife çekildi)." };
        }
    }
}
