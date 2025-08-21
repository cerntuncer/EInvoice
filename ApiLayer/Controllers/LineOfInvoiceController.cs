using BusinessLogicLayer.Handler.LineOfInvoiceHandler.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LineOfInvoiceController : ControllerBase
    {
        private readonly ILogger<LineOfInvoiceController> _logger;
        private readonly IMediator _mediator;

        public LineOfInvoiceController(ILogger<LineOfInvoiceController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost(Name = "CreateLineOfInvoice")]
        public async Task<IActionResult> Create(CreateLineOfInvoiceHandleRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Error)
                return UnprocessableEntity(result);
            return Ok(result);
        }
        // GET: /LineOfInvoice/{id}
        [HttpGet("{id}", Name = "GetLineOfInvoiceById")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _mediator.Send(new GetLineOfInvoiceByIdHandleRequest { Id = id });

            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }
        // 🔄 UPDATE
        [HttpPut]
        public async Task<IActionResult> Update(UpdateLineOfInvoiceHandleRequest request)
        {

            var result = await _mediator.Send(request);
            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }

        // ❌ DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _mediator.Send(new DeleteLineOfInvoiceHandleRequest { Id = id });
            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }

    }
}
