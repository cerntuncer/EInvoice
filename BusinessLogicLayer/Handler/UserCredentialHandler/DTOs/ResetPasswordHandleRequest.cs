using MediatR;

namespace BusinessLogicLayer.Handler.UserCredentialHandler.DTOs
{
    public sealed class ResetPasswordHandleRequest : IRequest<ResetPasswordHandleResponse>
    {
        public string Email { get; set; } = null!;
        public string IdentityNumber { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}

