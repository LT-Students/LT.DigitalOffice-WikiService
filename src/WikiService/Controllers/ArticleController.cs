using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Mvc;
using LT.DigitalOffice.WikiService.Business.Commands.Article.Interfaces;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;
using Microsoft.AspNetCore.JsonPatch;

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
  }

  //[HttpPatch("edit")]
 /* public async Task<OperationResultResponse<bool>> EditAsync(
  [FromServices] IEditArticleCommand command,
  [FromQuery] Guid articleId,
  [FromBody] JsonPatchDocument<EditArticleRequest> request)
  {
    return await command.ExecuteAsync(articleId, request);
  }*/
}