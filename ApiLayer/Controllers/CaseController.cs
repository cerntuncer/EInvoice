using BusinessLogicLayer.Handler.CaseHandler;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CaseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // POST: /Case
        [HttpPost]
        public async Task<IActionResult> CreateCase(CreateCaseHandleRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        // GET: /Case/{id}
        [HttpGet("{id}", Name = "GetCaseById")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _mediator.Send(new GetCaseByIdHandleRequest { Id = id });

            if (result.Error)
                return NotFound(result);

            return Ok(result);
        }

    }
}
