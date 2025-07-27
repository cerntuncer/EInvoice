using BusinessLogicLayer.Handler.UserHandler;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMediator _mediator;
        public UserController(ILogger<UserController> logger, IMediator mediator)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost(Name = "CreateUser")]
        public async Task<IActionResult> Create(CreateUserHandleRequest request)
        {
            var data = await _mediator.Send(request);
            return Ok(data);
        }
    }
}
