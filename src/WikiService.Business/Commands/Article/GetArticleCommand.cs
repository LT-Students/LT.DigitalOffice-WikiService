using System.Net;
using System.Threading.Tasks;
using System;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.WikiService.Data.Interfaces;
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

    public async Task<OperationResultResponse<ArticleResponse>> ExecuteAsync(Guid articleId)
    {
      OperationResultResponse<ArticleResponse> response = new();

      response.Body = _articleResponseMapper.Map(await _articleRepository.GetAsync(articleId));

      return response.Body == null
        ? _responseCreator.CreateFailureResponse<ArticleResponse>(HttpStatusCode.NotFound)
        : response;
    }
  }
}
