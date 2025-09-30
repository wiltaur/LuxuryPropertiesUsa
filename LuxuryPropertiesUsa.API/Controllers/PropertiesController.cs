using MediatR;
using Microsoft.AspNetCore.Mvc;
using LuxuryPropertiesUsa.Application.Features.Properties.Queries;

namespace LuxuryPropertiesUsa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PropertiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetPropertiesQuery());
        return Ok(result);
    }
}