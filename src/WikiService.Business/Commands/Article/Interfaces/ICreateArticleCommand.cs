using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Article.Interfaces
{
  [AutoInject]
  public interface ICreateArticleCommand
  {
    Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateArticleRequest request);
  }
}
