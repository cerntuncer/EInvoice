using MediatR;

namespace BusinessLogicLayer.Handler.UserCredentialHandler.DTOs
{
    public sealed class ChangePasswordHandleRequest : IRequest<ChangePasswordHandleResponse>
    {
        public string Email { get; set; } = null!;
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}

