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
            // GET: /Address/{id}
            [HttpGet("{id}", Name = "GetAddressById")]
            public async Task<IActionResult> GetById(long id)
            {
                var request = new GetAddressByIdHandleRequest { Id = id };

                var result = await _mediator.Send(request);

                if (result.Error)
                    return NotFound(result); // 404 - bulunamadı veya hata varsa

                return Ok(result); // 200 - başarılı
            }
            // GET: /Address/ByPerson/{personId}
            [HttpGet("ByPerson/{personId}", Name = "GetAddressesByPersonId")]
            public async Task<IActionResult> GetByPersonId(long personId)
            {
                var request = new GetAddressesByPersonIdRequest
                {
                    PersonId = personId
                };

                var result = await _mediator.Send(request);

                if (result.Error)
                    return NotFound(result); // 404 - Adres bulunamadı

                return Ok(result); // 200 - Başarılı
            }
        // PUT: /Address
        [HttpPut(Name = "UpdateAddress")]
        public async Task<IActionResult> Update(UpdateAddressHandleRequest request)
        {
            var result = await _mediator.Send(request);

            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }
        // DELETE: /Address/{id}?personId=5
        [HttpDelete("{id}", Name = "DeleteAddress")]
        public async Task<IActionResult> Delete(long id, [FromQuery] long personId)
        {
            var result = await _mediator.Send(new DeleteAddressHandleRequest
            {
                Id = id,
                PersonId = personId
            });

            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }


    }
}
