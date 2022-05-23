using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;

namespace LT.DigitalOffice.WikiService.Business.Commands.Article.Interfaces
{
  [AutoInject]
  public interface IEditArticleCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(Guid articleId, JsonPatchDocument<EditArticleRequest> patch);
  }
}