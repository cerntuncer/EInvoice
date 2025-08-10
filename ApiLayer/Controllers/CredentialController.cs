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
        public CredentialController(IMediator mediator) => _mediator = mediator;

        // POST: /Credential
        [HttpPost(Name = "CreateCredential")]
        [ProducesResponseType(typeof(CreateUserCredentialHandleResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateUserCredentialHandleRequest req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _mediator.Send(req);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                // ör: user yok, email zaten var, aynı kullanıcıya ikinci credential vb.
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
