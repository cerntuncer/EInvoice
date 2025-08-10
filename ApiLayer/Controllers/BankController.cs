using BusinessLogicLayer.Handler.BankHandler.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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
        // PUT: /Bank
        [Authorize]
        [HttpPut(Name = "UpdateBank")]
        public async Task<IActionResult> Update(UpdateBankHandleRequest request)
        {
            var result = await _mediator.Send(request);

            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }
        // DELETE: /Bank/{id}?currentId=5
        [Authorize]
        [HttpDelete("{id}", Name = "DeleteBank")]
        public async Task<IActionResult> Delete(long id, [FromQuery] long currentId)
        {
            var result = await _mediator.Send(new DeleteBankHandleRequest
            {
                Id = id,
                CurrentId = currentId
            });

            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }


    }
}
