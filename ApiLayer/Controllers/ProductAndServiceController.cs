﻿using BusinessLogicLayer.Handler.ProductAndServiceHandler;
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

        [HttpPost(Name = "CreateProductAndService")]
        public async Task<IActionResult> Create(CreateProductAndServiceHandleRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
