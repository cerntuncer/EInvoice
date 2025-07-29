using BusinessLogicLayer.Handler.BankHandler;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BankController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBank(CreateBankHandleRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
