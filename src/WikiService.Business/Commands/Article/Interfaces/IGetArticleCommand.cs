using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;
using LT.DigitalOffice.WikiService.Models.Dto.Responses.Article;

namespace LT.DigitalOffice.WikiService.Business.Commands.Article.Interfaces
{
  [AutoInject]
  public interface IGetArticleCommand
  {
    Task<OperationResultResponse<ArticleResponse>> ExecuteAsync(GetArticleRequest request);
  }
}
