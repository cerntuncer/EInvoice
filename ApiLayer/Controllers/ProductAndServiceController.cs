using BusinessLogicLayer.Handler.ProductAndServiceHandler;
using BusinessLogicLayer.Handler.ProductAndServiceHandler.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductAndServiceController : ControllerBase
    {
        private readonly ILogger<ProductAndServiceController> _logger;
        private readonly IMediator _mediator;

        public ProductAndServiceController(ILogger<ProductAndServiceController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        // POST: /ProductAndService
        [Authorize]
        [HttpPost(Name = "CreateProductAndService")]
        public async Task<IActionResult> Create(CreateProductAndServiceHandleRequest request)
        {
            var sub = User.FindFirst("sub")?.Value;
            if (!long.TryParse(sub, out var userId)) return Unauthorized();
            request.UserId = userId;

            var result = await _mediator.Send(request);
            if (result.Error)
                return UnprocessableEntity(result);
            return Ok(result);
        }
        // GET: /ProductAndService/{id}
        [Authorize]
        [HttpGet("{id}", Name = "GetProductAndServiceById")]
        public async Task<IActionResult> GetById(long id)
        {
            var sub = User.FindFirst("sub")?.Value;
            if (!long.TryParse(sub, out var userId)) return Unauthorized();

            var result = await _mediator.Send(new GetProductAndServiceByIdHandleRequest { Id = id, UserId = userId });

            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }
        // GET: /ProductAndService/Mine
        [Authorize]
        [HttpGet("Mine", Name = "GetMyProductsAndServices")]
        public async Task<IActionResult> GetMine()
        {
            var sub = User.FindFirst("sub")?.Value;
            if (!long.TryParse(sub, out var userId)) return Unauthorized();

            var result = await _mediator.Send(new BusinessLogicLayer.Handler.ProductAndServiceHandler.Queries.GetMyProductsAndServicesHandleRequest
            {
                UserId = userId
            });

            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }
        //PUT :/ProductAndService/{id}
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductAndServiceHandleRequest request)
        {
            var sub = User.FindFirst("sub")?.Value;
            if (!long.TryParse(sub, out var userId)) return Unauthorized();
            request.UserId = userId;

            var result = await _mediator.Send(request);

            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }
        // DELETE :/ProductAndService/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var sub = User.FindFirst("sub")?.Value;
            if (!long.TryParse(sub, out var userId)) return Unauthorized();

            var result = await _mediator.Send(new DeleteProductAndServiceHandleRequest { Id = id, UserId = userId });

            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }



    }
}
