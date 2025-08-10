using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Handler.UserCredentialHandler.DTOs;

namespace ApiLayer.Controllers
{
    [ApiController]
    [Route("[controller]")] // -> /Credential
    public class CredentialController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CredentialController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: /Credential
        [HttpPost(Name = "CreateCredential")]
        [ProducesResponseType(typeof(CreateUserCredentialHandleResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(CreateUserCredentialHandleRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Error)
                return UnprocessableEntity(result);
            return Ok(result);
        }
    }
}
