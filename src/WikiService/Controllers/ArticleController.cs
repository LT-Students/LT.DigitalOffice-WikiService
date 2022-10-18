using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.WikiService.Business.Commands.Article.Edit;
using LT.DigitalOffice.WikiService.Business.Commands.Article.Get;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class ArticleController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly IAccessValidator _accessValidator;

    public ArticleController(
      IMediator mediator,
      IAccessValidator accessValidator)
    {
      _mediator = mediator;
      _accessValidator = accessValidator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync(
      [FromBody] CreateArticleRequest request,
      CancellationToken ct)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveWiki))
      {
        return StatusCode(403);
      }

      return Created("/article", await _mediator.Send(request, ct));
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetAsync(
      [FromQuery] Guid articleId,
      CancellationToken ct)
    {
      return Ok(await _mediator.Send(
        new GetArticleRequest { Id = articleId },
        ct));
    }

    [HttpPatch("edit")]
    public async Task<IActionResult> EditAsync(
      [FromQuery] Guid articleId,
      [FromBody] JsonPatchDocument<EditArticleRequest> request,
      CancellationToken ct)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveWiki))
      {
        return StatusCode(403);
      }

      return Ok(await _mediator.Send(
        new EditSpecificArticleRequest { Id = articleId, Request = request },
        ct));
    }
  }
}