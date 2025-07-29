using BusinessLogicLayer.Handler.InvoiceHandler;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<InvoiceController> _logger;

        public InvoiceController(IMediator mediator, ILogger<InvoiceController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost(Name = "CreateInvoice")]
        public async Task<IActionResult> Create(CreateInvoiceHandleRequest request)
        {
            var result = await _mediator.Send(request);

            if (result.Error)
            {
                _logger.LogWarning("Fatura oluşturulamadı: {Message}", result.Message);
                return BadRequest(result);
            }

            _logger.LogInformation("Fatura başarıyla oluşturuldu.");
            return Ok(result);
        }
    }
}
