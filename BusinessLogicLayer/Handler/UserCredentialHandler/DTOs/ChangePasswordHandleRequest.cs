using MediatR;

namespace BusinessLogicLayer.Handler.UserCredentialHandler.DTOs
{
    public sealed class ChangePasswordHandleRequest : IRequest<ChangePasswordHandleResponse>
    {
        public string Email { get; set; } = null!;
        public string CurrentPassword { get; set; } = string.Empty; // optional for identity flow
        public string NewPassword { get; set; } = null!;

        // Identity verification for unauthenticated forgot flow
        public string IdentityNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}
