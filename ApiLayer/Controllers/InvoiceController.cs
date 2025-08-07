using BusinessLogicLayer.Handler.InvoiceHandler.DTOs;
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
        // GET: /Invoice/{id}
        [HttpGet("{id}", Name = "GetInvoiceById")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _mediator.Send(new GetInvoiceByIdHandleRequest { Id = id });

            if (result.Error)
                return NotFound(result);

            return Ok(result);
        }
        // PUT: api/Invoice
        [HttpPut(Name = "UpdateInvoice")]
        public async Task<IActionResult> Update(UpdateInvoiceHandleRequest request)
        {
            var result = await _mediator.Send(request);

            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }

        // DELETE: api/Invoice/{id}
        [HttpDelete("{id}", Name = "DeleteInvoice")]
        public async Task<IActionResult> Delete(long id)
        {
            var request = new DeleteInvoiceHandleRequest { Id = id };
            var result = await _mediator.Send(request);

            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
