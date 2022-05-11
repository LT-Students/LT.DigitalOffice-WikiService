using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Mvc;
using LT.DigitalOffice.WikiService.Business.Commands.Article.Interfaces;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;

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
}

