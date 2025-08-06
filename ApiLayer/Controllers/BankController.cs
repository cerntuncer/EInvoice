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
        // POST: /Bank
        [HttpPost]
        public async Task<IActionResult> CreateBank(CreateBankHandleRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        // GET: /Bank/{id}
        [HttpGet("{id}", Name = "GetBankById")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _mediator.Send(new GetBankByIdHandleRequest { Id = id });

            if (result.Error)
                return NotFound(result);

            return Ok(result);
        }
    }
}
