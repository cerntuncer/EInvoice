using BusinessLogicLayer.Handler.CustomerSupplierHandler;
using BusinessLogicLayer.Handler.CustomerSupplierHandler.DTOs;
using BusinessLogicLayer.Handler.UserHandler;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CustomerSupplierController : ControllerBase
    {
        private readonly ILogger<CustomerSupplierController> _logger;
        private readonly IMediator _mediator;

        public CustomerSupplierController(ILogger<CustomerSupplierController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost(Name = "CreateCustomerSupplier")]
        public async Task<IActionResult> Create(CreateCustomerSupplierHandleRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Error)
                return UnprocessableEntity(result);
            return Ok(result);
        }

        // GET: /CustomerSupplier/List
        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var result = await _mediator.Send(new GetCustomerSuppliersHandleRequest());
            if (result.Error)
                return UnprocessableEntity(result);
            return Ok(result);
        }
        // GET: /CustomerSupplier/{id}
        [HttpGet("{id}", Name = "GetCustomerSupplierById")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _mediator.Send(new GetCustomerSupplierByIdHandleRequest { Id = id });

            if (result.Error)
                return UnprocessableEntity(result);

            return Ok(result);
        }
        // PUT: /CustomerSupplier/Update
        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdateCustomerSupplierHandleRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Error)
                return UnprocessableEntity(result);
            return Ok(result);
        }

        // DELETE: /CustomerSupplier/Delete/{id}
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var request = new DeleteCustomerSupplierHandleRequest { Id = id };
            var result = await _mediator.Send(request);
            if (result.Error)
                return UnprocessableEntity(result);
            return Ok(result);
        }


    }
}

