using BusinessLogicLayer.Handler.UserCredentialHandler.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;

namespace ApiLayer.Controllers
{
    [ApiController]
    [Route("[controller]")] // /Credential
    public class CredentialController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CredentialController(IMediator mediator) => _mediator = mediator;

        [Authorize]
        [HttpPost(Name = "CreateCredential")]
        public async Task<IActionResult> Create([FromBody] CreateUserCredentialHandleRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }
        [HttpGet("Me")]
        [Authorize]
        public async Task<IActionResult> GetMyCredential()
        {
            var email = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value
                        ?? User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(email))
                return Unauthorized(new { error = true, message = "Email bulunamadı, tekrar giriş yapın." });

            var result = await _mediator.Send(
                new GetUserCredentialByEmailHandleRequest { Email = email }
            );

            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }

        [HttpPost("ChangePassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordHandleRequest request)
        {
            var emailFromToken = User.FindFirst(ClaimTypes.Email)?.Value
                        ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value
                        ?? User.Identity?.Name;

            if (!string.IsNullOrWhiteSpace(emailFromToken))
            {
                request.Email = emailFromToken;
            }
            else if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new { error = true, message = "Email zorunludur." });
            }
            var result = await _mediator.Send(request);
            if (result.Error)
                return UnprocessableEntity(result);
            return Ok(result);
        }
    }
}