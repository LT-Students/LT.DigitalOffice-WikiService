using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;
using LT.DigitalOffice.WikiService.Models.Dto.Responses.Article;
using LT.DigitalOffice.WikiService.Mappers.Models.Interfaces;
using LT.DigitalOffice.WikiService.Business.Commands.Article.Interfaces;

namespace LT.DigitalOffice.WikiService.Business.Commands.Article
{
  public class GetArticleCommand : IGetArticleCommand
  {
    private readonly IArticleRepository _articleRepository;
    private readonly IArticleResponseMapper _articleResponseMapper;
    private readonly IResponseCreator _responseCreator;

    public GetArticleCommand(
      IArticleRepository articleRepository,
      IArticleResponseMapper articleResponseMapper,
      IResponseCreator responseCreator)
    {
      _articleRepository = articleRepository;
      _articleResponseMapper = articleResponseMapper;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<ArticleResponse>> ExecuteAsync(GetArticleRequest request)
    {
      if (request.ArticleId is null)
      {
        return _responseCreator.CreateFailureResponse<ArticleResponse>(HttpStatusCode.BadRequest,
          new List<string> { "You must specify 'ArticleId'." });
      }

      OperationResultResponse<ArticleResponse> response = new();

      DbArticle dbArticle = await _articleRepository.GetAsync(request);

      response.Body = _articleResponseMapper.Map(dbArticle);
      response.Status = OperationResultStatusType.FullSuccess;

      return response;
    }
  }
}
