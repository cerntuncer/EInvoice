using BusinessLogicLayer.Handler.CustomerSupplierHandler;
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
    }
}

