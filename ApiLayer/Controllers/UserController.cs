using BusinessLogicLayer.Handler.UserHandler.Commands;
using BusinessLogicLayer.Handler.UserHandler.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        // POST: /User

        [Authorize]
        [HttpPost(Name = "CreateUser")]
        public async Task<IActionResult> Create(CreateUserHandleRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Error)
                return UnprocessableEntity(result);
            return Ok(result);
        }
        // GET: /User/{id}
        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _mediator.Send(new GetUserByIdHandleRequest { Id = id });
            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }
        // GET: /User/WithPerson/{id}
        [HttpGet("WithPerson/{id}", Name = "GetUserWithPersonById")]
        public async Task<IActionResult> GetWithPersonById(long id)
        {
            var result = await _mediator.Send(new GetUserWithPersonByIdHandleRequest { Id = id });
            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }
        // GET: /User/WithPerson
        [HttpGet("WithPerson", Name = "GetUsersWithPersonList")]
        public async Task<IActionResult> GetUsersWithPersonList()
        {
            var result = await _mediator.Send(new GetUsersWithPersonListHandleRequest());
            if (result.Error)
                return UnprocessableEntity(result);
            return Ok(result);
        }
        // PUT: /User
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateUserHandleRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Error)
                return UnprocessableEntity(result);
            return Ok(result.Message);
        }
        // DELETE: /User/{id}?personId=5
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _mediator.Send(new DeleteUserHandleRequest { Id = id });
            if (result.Error)
                return UnprocessableEntity(result); 
            return Ok(result.Message);
        }
       
    }
}
