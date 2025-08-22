using BusinessLogicLayer.Handler.CustomerSupplierHandler.DTOs;
using BusinessLogicLayer.Handler.UserCredentialHandler.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiLayer.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CustomerSupplierController : ControllerBase
    {
        private readonly ILogger<CustomerSupplierController> _logger;
        private readonly IMediator _mediator;

        public CustomerSupplierController(ILogger<CustomerSupplierController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        private async Task<long?> GetCurrentUserIdAsync()
        {
            var sub = User.FindFirst("sub")?.Value;
            if (long.TryParse(sub, out var subUserId))
                return subUserId;

            var email = User.FindFirst(ClaimTypes.Email)?.Value ?? User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var cred = await _mediator.Send(new GetUserCredentialByEmailHandleRequest { Email = email });
            if (cred.Error)
                return null;
            return cred.UserId;
        }

        [Authorize]
        [HttpPost(Name = "CreateCustomerSupplier")]
        public async Task<IActionResult> Create(CreateCustomerSupplierHandleRequest request)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null) return Unauthorized();
            request.UserId = userId.Value;

            var result = await _mediator.Send(request);
            if (result.Error)
                return UnprocessableEntity(result);
            return Ok(result);
        }

        // GET: /CustomerSupplier/List
        [Authorize]
        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null) return Unauthorized();

            var result = await _mediator.Send(new GetCustomerSuppliersHandleRequest { UserId = userId.Value });
            if (result.Error)
                return UnprocessableEntity(result);
            return Ok(result);
        }
        // GET: /CustomerSupplier/{id}
        [Authorize]
        [HttpGet("{id}", Name = "GetCustomerSupplierById")]
        public async Task<IActionResult> GetById(long id)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null) return Unauthorized();

            var result = await _mediator.Send(new GetCustomerSupplierByIdHandleRequest { Id = id, UserId = userId.Value });

            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }
        // PUT: /CustomerSupplier/Update
        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdateCustomerSupplierHandleRequest request)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null) return Unauthorized();
            request.UserId = userId.Value;

            var result = await _mediator.Send(request);
            if (result.Error)
                return UnprocessableEntity(result);
            return Ok(result);
        }

        // DELETE: /CustomerSupplier/Delete/{id}
        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null) return Unauthorized();

            var request = new DeleteCustomerSupplierHandleRequest { Id = id, UserId = userId.Value };
            var result = await _mediator.Send(request);
            if (result.Error)
                return UnprocessableEntity(result);
            return Ok(result);
        }


    }
}