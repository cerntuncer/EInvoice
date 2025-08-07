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
            var data = await _mediator.Send(request);
            return Ok(data);
        }
        // GET: /CustomerSupplier/{id}
        [HttpGet("{id}", Name = "GetCustomerSupplierById")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _mediator.Send(new GetCustomerSupplierByIdHandleRequest { Id = id });

            if (result.Error)
                return NotFound(result);

            return Ok(result);
        }
        // PUT: /CustomerSupplier/Update
        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdateCustomerSupplierHandleRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Error)
                return BadRequest(result);
            return Ok(result);
        }

        // DELETE: /CustomerSupplier/Delete/{id}
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var request = new DeleteCustomerSupplierHandleRequest { Id = id };
            var result = await _mediator.Send(request);
            if (result.Error)
                return BadRequest(result);
            return Ok(result);
        }


    }
}

