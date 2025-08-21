using BusinessLogicLayer.Handler.ProductAndServiceHandler;
using BusinessLogicLayer.Handler.ProductAndServiceHandler.DTOs;
using BusinessLogicLayer.Handler.UserCredentialHandler.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiLayer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductAndServiceController : ControllerBase
    {
        private readonly ILogger<ProductAndServiceController> _logger;
        private readonly IMediator _mediator;

        public ProductAndServiceController(ILogger<ProductAndServiceController> logger, IMediator mediator)
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
        // POST: /ProductAndService
        [Authorize]
        [HttpPost(Name = "CreateProductAndService")]
        public async Task<IActionResult> Create(CreateProductAndServiceHandleRequest request)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null) return Unauthorized();
            request.UserId = userId.Value;

            var result = await _mediator.Send(request);
            if (result.Error)
                return UnprocessableEntity(result);
            return Ok(result);
        }
        // GET: /ProductAndService/{id}
        [Authorize]
        [HttpGet("{id}", Name = "GetProductAndServiceById")]
        public async Task<IActionResult> GetById(long id)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null) return Unauthorized();

            var result = await _mediator.Send(new GetProductAndServiceByIdHandleRequest { Id = id, UserId = userId.Value });

            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }
       
        //PUT :/ProductAndService/{id}
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductAndServiceHandleRequest request)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null) return Unauthorized();
            request.UserId = userId.Value;

            var result = await _mediator.Send(request);

            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }
        // DELETE :/ProductAndService/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null) return Unauthorized();

            var result = await _mediator.Send(new DeleteProductAndServiceHandleRequest { Id = id, UserId = userId.Value });

            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }



    }
}