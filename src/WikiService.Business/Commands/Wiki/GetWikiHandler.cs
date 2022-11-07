using LT.DigitalOffice.WikiService.Data.Provider;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Wiki
{
  public class GetWikiHandler : IRequestHandler<GetWikiFilter, List<RubricData>>
  {
    private readonly IDataProvider _dbContext;
    private readonly IMemoryCache _cache;

    public GetWikiHandler(
      IDataProvider dbContext,
      IMemoryCache cache)
    {
      _dbContext = dbContext;
      _cache = cache;
    }

    public async Task<List<RubricData>> Handle(GetWikiFilter request, CancellationToken ct)
    {
      List<RubricData> wikiTreeCache = _cache.Get<List<RubricData>>(request.includeDeactivated 
        ? CacheKeys.WikiTreeWithDeactivated 
        : CacheKeys.WikiTreeWithoutDeactivated);

      if (wikiTreeCache is null)
      {
        List<RubricData> rubrics = await _dbContext.Rubrics.Include(rubric => rubric.Articles)
          .Where(x => x.IsActive == true || x.IsActive == !request.includeDeactivated).Select(x => new RubricData
            {
              Id = x.Id,
              Name = x.Name,
              IsActive = x.IsActive,
              ParentId = x.ParentId,
              Articles = x.Articles.Select(article => new ArticleData
              {
                Id = article.Id,
                Name = article.Name,
                isActive = article.IsActive
              }).ToList()
            }).ToListAsync();

        foreach (RubricData rubric in rubrics)
        {
          rubric.Children = rubrics.Where(r => r.ParentId == rubric.Id).ToList();
        }

        rubrics.RemoveAll(x => x.ParentId is not null);

        wikiTreeCache = _cache.Set(
          request.includeDeactivated ? CacheKeys.WikiTreeWithDeactivated : CacheKeys.WikiTreeWithoutDeactivated,
          rubrics);
      }

       return wikiTreeCache;
    }
  }
}
