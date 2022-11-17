using System.Threading.Tasks;
using MediatR;
using System.Threading;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LT.DigitalOffice.WikiService.Business.Commands.Article.Get
{
  public class GetArticleHandler : IRequestHandler<GetArticleRequest, ArticleResponse>
  {
    private readonly IDataProvider _provider;

    public GetArticleHandler(
      IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task<ArticleResponse> Handle(GetArticleRequest request, CancellationToken ct)
    {
      DbArticle article = await _provider.Articles.Include(a => a.Files)
        .FirstOrDefaultAsync(article => article.Id == request.Id);

      return article is null 
        ? null
        : new ArticleResponse
          {
            Id = article.Id,
            Name = article.Name,
            Content = article.Content,
            IsActive = article.IsActive,
            RubricId = article.RubricId,
            Position = article.Position,
            Files = article.Files
              ?.Select(f => f.FileId).ToList(),
          };
    }
  }
}
