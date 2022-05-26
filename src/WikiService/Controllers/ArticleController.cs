using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Business.Commands.Article.Interfaces;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;
using LT.DigitalOffice.WikiService.Models.Dto.Responses.Article;

namespace LT.DigitalOffice.WikiService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class ArticleController: ControllerBase
  {
    [HttpPost("create")]
    public async Task<OperationResultResponse<Guid?>> CreateAsync(
      [FromServices] ICreateArticleCommand command,
      [FromBody] CreateArticleRequest request)
    {
      return await command.ExecuteAsync(request);
    }

    [HttpGet("get")]
    public async Task<OperationResultResponse<ArticleResponse>> GetAsync(
      [FromServices] IGetArticleCommand command,
      [FromQuery] Guid articleId)
    {
      return await command.ExecuteAsync(articleId);
    }
  }
}