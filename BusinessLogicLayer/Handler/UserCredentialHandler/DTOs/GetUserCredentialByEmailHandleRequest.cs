using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Handler.UserCredentialHandler.DTOs
{
    public class GetUserCredentialByEmailHandleRequest : IRequest<GetUserCredentialByEmailHandleResponse>
    {
        public string Email {  get; set; }
    }
}
