using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.UserCredentialHandler.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Handler.UserCredentialHandler.Queries
{
    public class GetUserCredentialByEmailHandle
      : IRequestHandler<GetUserCredentialByEmailHandleRequest, GetUserCredentialByEmailHandleResponse>
    {
        private readonly IUserCredentialRepository _userCredentialRepo;

        public GetUserCredentialByEmailHandle(IUserCredentialRepository userCredentialRepo)
        {
            _userCredentialRepo = userCredentialRepo;
        }

        public async Task<GetUserCredentialByEmailHandleResponse> Handle(GetUserCredentialByEmailHandleRequest request,CancellationToken cancellationToken)
        {
            var credential =  _userCredentialRepo.FirstOrDefault(
                x => x.Email == request.Email);

            if (credential is null)
            {
                return new GetUserCredentialByEmailHandleResponse
                {
                    Error = true,
                    Message = "UserCredential bulunamadı."
                };
            }

            return new GetUserCredentialByEmailHandleResponse
            {
                Error = false,
                Message = "Başarılı",
                UserId = credential.UserId,
                CredentialId = credential.Id,
                Email = credential.Email
            };
        }
    }
}
