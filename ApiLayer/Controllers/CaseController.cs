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

        [HttpPost]
        public async Task<IActionResult> CreateCase(CreateCaseHandleRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
