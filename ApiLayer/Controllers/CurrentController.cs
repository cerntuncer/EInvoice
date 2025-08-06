using BusinessLogicLayer.Handler.CurrentHandler;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CurrentController : ControllerBase
{
    private readonly IMediator _mediator;

    public CurrentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = "CreateCurrent")]
    public async Task<IActionResult> Create(CreateCurrentHandleRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
    // GET: /Current/{id}
    [HttpGet("{id}", Name = "GetCurrentById")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _mediator.Send(new GetCurrentByIdHandleRequest { Id = id });

        if (result.Error)
            return NotFound(result);

        return Ok(result);
    }
    // GET: /Current/ByUser/{userId}
    [HttpGet("ByUser/{userId}", Name = "GetCurrentsByUserId")]
    public async Task<IActionResult> GetByUserId(long userId)
    {
        var result = await _mediator.Send(new GetCurrentsByUserIdHandleRequest { UserId = userId });

        if (result.Error)
            return NotFound(result);

        return Ok(result);
    }

}
