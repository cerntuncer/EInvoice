using BusinessLogicLayer.Handler.ProductAndServiceHandler;
using BusinessLogicLayer.Handler.ProductAndServiceHandler.DTOs;
using MediatR;
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
        [HttpPost(Name = "CreateProductAndService")]
        public async Task<IActionResult> Create(CreateProductAndServiceHandleRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Error)
                return UnprocessableEntity(result);
            return Ok(result);
        }
        // GET: /ProductAndService/{id}
        [HttpGet("{id}", Name = "GetProductAndServiceById")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _mediator.Send(new GetProductAndServiceByIdHandleRequest { Id = id });

            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }
        //PUT :/ProductAndService/{id}
        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductAndServiceHandleRequest request)
        {

            var result = await _mediator.Send(request);

            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }
        // DELETE :/Person/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _mediator.Send(new DeleteProductAndServiceHandleRequest { Id = id });

            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }



    }
}
