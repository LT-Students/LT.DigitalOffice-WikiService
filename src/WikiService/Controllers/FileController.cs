using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.WikiService.Business.Commands.Article.Edit;
using LT.DigitalOffice.WikiService.Business.Commands.Article.Get;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System;
using MediatR;
using LT.DigitalOffice.WikiService.Business.Commands.File.Remove;
using LT.DigitalOffice.WikiService.Business.Commands.File.Find;
using DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.WikiService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class FileController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly IAccessValidator _accessValidator;

    public FileController(
      IMediator mediator,
      IAccessValidator accessValidator)
    {
      _mediator = mediator;
      _accessValidator = accessValidator;
    }

    [HttpGet("find")]
    public async Task<IActionResult> GetAsync(
      [FromBody] FileFindFilter filter,
      CancellationToken ct)
    {
      FindResult<FileInfo> result = await _mediator.Send(
        filter,
        ct);

      return result is null
        ? StatusCode(404)
        : Ok(result);
    }

    [HttpDelete("remove")]
    public async Task<IActionResult> EditAsync(
      [FromBody] RemoveFilesRequest request,
      CancellationToken ct)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveWiki))
      {
        return StatusCode(403);
      }

      return Ok(await _mediator.Send(request, ct));
    }
  }
}
