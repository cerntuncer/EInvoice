using BusinessLogicLayer.Handler.LineOfInvoiceHandler;
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
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
