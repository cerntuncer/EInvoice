using BusinessLogicLayer.Handler.PersonHandler;
using MediatR;
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

        [HttpPost(Name = "CreatePerson")]
        public async Task<IActionResult> Create(CreatePersonHandleRequest request)
        {
            var data = await _mediator.Send(request);
            return Ok(data);
        }
        // GET: /Person/{id}
        [HttpGet("{id}", Name = "GetPersonById")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _mediator.Send(new GetPersonByIdHandleRequest { Id = id });

            if (result.Error)
                return NotFound(result);

            return Ok(result);
        }

    }
}
