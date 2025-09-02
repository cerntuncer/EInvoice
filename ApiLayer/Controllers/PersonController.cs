using BusinessLogicLayer.Handler.PersonHandler;
using BusinessLogicLayer.Handler.PersonHandler.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IMediator _mediator;
        public PersonController(ILogger<PersonController> logger, IMediator mediator)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Authorize]
        [HttpPost(Name = "CreatePerson")]
        public async Task<IActionResult> Create(CreatePersonHandleRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Error)
                return UnprocessableEntity(result);
            return Ok(result);
        }
        // GET: /Person/{id}
        [HttpGet("{id}", Name = "GetPersonById")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _mediator.Send(new GetPersonByIdHandleRequest { Id = id });

            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }
        // PUT :/Person/{id}
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(UpdatePersonHandleRequest request)
        {

            var result = await _mediator.Send(request);

            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }
        // DELETE :/Person/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var request = new DeletePersonHandleRequest { Id = id };
            var result = await _mediator.Send(request);

            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }




    }
}