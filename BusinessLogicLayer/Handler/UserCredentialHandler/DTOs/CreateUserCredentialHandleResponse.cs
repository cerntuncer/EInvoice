using System;

namespace BusinessLogicLayer.Handler.UserCredentialHandler.DTOs
{
    public sealed class CreateUserCredentialHandleResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public long CredentialId { get; set; }
        public long UserId { get; set; }
        public string Email { get; set; } = null!;
        public bool EmailConfirmed { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
