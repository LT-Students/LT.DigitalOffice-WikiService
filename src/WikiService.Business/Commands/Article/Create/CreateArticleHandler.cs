using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.WikiService.Business.Commands.Wiki;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Article.Create
{
  public class CreateArticleHandler : IRequestHandler<CreateArticleRequest, Guid?>
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDataProvider _provider;
    private readonly IMemoryCache _cache;

    private async Task<Guid?> CreateAsync(DbArticle dbArticle, CancellationToken ct)
    {
      if (dbArticle is null)
      {
        return null;
      }

      int? position = await _provider.Articles
        .Where(x => x.RubricId == dbArticle.RubricId)
        .MaxAsync(x => (int?)x.Position);
      dbArticle.Position = !position.HasValue ? 1 : position.Value + 1;

      await _provider.Articles.AddAsync(dbArticle, ct);
      await _provider.SaveAsync();

      return dbArticle.Id;
    }

    private DbArticle Map(CreateArticleRequest request)
    {
      if (request is null)
      {
        return null;
      }

      return new DbArticle
      {
        Id = Guid.NewGuid(),
        Name = request.Name,
        Content = request.Content,
        RubricId = request.RubricId,
        IsActive = true,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.UtcNow
      };
    }

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
      _cache.Remove(CacheKeys.WikiTreeWithDeactivated);
      _cache.Remove(CacheKeys.WikiTreeWithoutDeactivated);

      return await CreateAsync(Map(request), ct);
    }
  }
}
