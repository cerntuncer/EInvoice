using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Handler.UserCredentialHandler.DTOs
{
    public class GetUserCredentialByEmailHandleResponse
    {
        public bool Error { get; set; }
        public string Message { get; set; }
        public long UserId { get; set; }
        public long CredentialId { get; set; }
        public string Email { get; set; }
    }
}
