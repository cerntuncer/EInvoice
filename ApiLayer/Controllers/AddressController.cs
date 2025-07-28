using BusinessLogicLayer.Handler.AddressHandler;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
   
        [ApiController]
        [Route("[controller]")]
        public class AddressController : ControllerBase
        {
            private readonly ILogger<AddressController> _logger;
            private readonly IMediator _mediator;

            // Constructor: Logger ve Mediator dependency injection ile alınır
            public AddressController(ILogger<AddressController> logger, IMediator mediator)
            {
                _logger = logger;
                _mediator = mediator;
            }

            // POST: /Address
            [HttpPost(Name = "CreateAddress")]
            public async Task<IActionResult> Create(CreateAddressHandleRequest request)
            {
                // MediatR ile handler'a gönder
                var result = await _mediator.Send(request);

                // Sonuca göre HTTP yanıtı döndür
                if (result.Error)
                    return BadRequest(result);

                return Ok(result);
            }
        }
}
