using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.WikiService.Business.Commands.Wiki;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Article.Create
{
  public class CreateArticleHandler : IRequestHandler<CreateArticleRequest, Guid?>
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDataProvider _provider;
    private readonly IMemoryCache _cache;

    public CreateArticleHandler(
      IHttpContextAccessor httpContextAccessor,
      IDataProvider provider,
      IMemoryCache cache)
    {
      _httpContextAccessor = httpContextAccessor;
      _provider = provider;
      _cache = cache;
    }

    public async Task<Guid?> Handle(CreateArticleRequest request, CancellationToken ct)
    {
      DbArticle article = new DbArticle
      {
        Id = Guid.NewGuid(),
        Name = request.Name,
        Content = request.Content,
        RubricId = request.RubricId,
        IsActive = true,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.UtcNow
      };

      await _provider.Articles.AddAsync(article, ct);
      await _provider.SaveAsync();

      _cache.Remove(CacheKeys.WikiTreeWithDeactivated);
      _cache.Remove(CacheKeys.WikiTreeWithoutDeactivated);

      return article.Id;
    }
  }
}
