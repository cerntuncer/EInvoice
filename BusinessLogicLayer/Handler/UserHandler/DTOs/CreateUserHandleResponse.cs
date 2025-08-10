using System;

namespace BusinessLogicLayer.Handler.UserHandler.DTOs
{
    public sealed class CreateUserHandleResponse
    {
        public bool Error { get; set; }
        public string? Message { get; set; }

        public long? Id { get; set; }                 // UserId
        public long? PersonId { get; set; }

        public long? CredentialId { get; set; }       // eğer int/long PK ise
        public string? Email { get; set; }
    }
}
