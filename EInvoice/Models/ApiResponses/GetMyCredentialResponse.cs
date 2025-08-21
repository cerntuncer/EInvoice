namespace PresentationLayer.Models.ApiResponses
{
    public class GetMyCredentialResponse
    {
        public bool Error { get; set; }
        public string Message { get; set; }
        public long UserId { get; set; }
        public long CredentialId { get; set; }
        public string Email { get; set; }
    }
}

