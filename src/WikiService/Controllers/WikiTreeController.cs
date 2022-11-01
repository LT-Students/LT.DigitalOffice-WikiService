using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.WikiService.Business.Commands.Wiki;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class WikiTreeController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly IAccessValidator _accessValidator;

    public WikiTreeController(
      IMediator mediator,
      IAccessValidator accessValidator)
    {
      _mediator = mediator;
      _accessValidator = accessValidator;
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetAsync(
      [FromQuery] GetWikiFilter request,
      CancellationToken ct)
    {
      if (request.includeDeactivated
        && !await _accessValidator.HasRightsAsync(Rights.AddEditRemoveWiki))
      {
        return StatusCode(403);
      }

      return Ok(await _mediator.Send(request, ct));
    }
  }
}
