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
}
