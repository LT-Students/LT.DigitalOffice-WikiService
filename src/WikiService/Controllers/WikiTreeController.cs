using LT.DigitalOffice.WikiService.Business.Commands.Wiki;
using LT.DigitalOffice.WikiService.Models.Dto.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class WikiTreeController : ControllerBase
  {
    private readonly IMediator _mediator;

    public WikiTreeController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetAsync(
      [FromQuery] GetWikiFilter request,
      CancellationToken ct)
    {
      return Ok(await _mediator.Send(request, ct));
    }
  }
}
