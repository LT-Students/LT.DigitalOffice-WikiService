using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.WikiService.Models.Dto.Models;
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
    private readonly WikiServiceDbContext _dbContext;
    private readonly IMemoryCache _cache;

    public GetWikiHandler(
      WikiServiceDbContext dbContext,
      IMemoryCache cache)
    {
      _dbContext = dbContext;
      _cache = cache;
    }

    public async Task<List<RubricData>> Handle(GetWikiFilter request, CancellationToken ct)
    {
      List<RubricData> wikiTreeCache = _cache.Get<List<RubricData>>(request.IncludeArchivals 
        ? CacheKeys.WikiTreeWithArchivals 
        : CacheKeys.WikiTreeWithoutArchivals);

      if (wikiTreeCache is null)
      {
        List<RubricData> rubrics = await _dbContext.Rubrics.AsQueryable().Include(rubric => rubric.Articles).Select(x => new RubricData
        {
          Id = x.Id,
          Name = x.Name,
          IsActive = x.IsActive,
          ParentId = x.ParentId,
          ArticlesNames = x.Articles.Select(article => article.Name).ToList()
        }).ToListAsync();

        if (!request.IncludeArchivals)
        {
          rubrics.RemoveAll(x => !x.IsActive);
        }

        foreach (RubricData rubric in rubrics)
        {
          rubric.Children = rubrics.Where(r => r.ParentId == rubric.Id).ToList();
        }

        rubrics.RemoveAll(x => x.ParentId is not null);

        wikiTreeCache = _cache.Set(
          request.IncludeArchivals ? CacheKeys.WikiTreeWithArchivals : CacheKeys.WikiTreeWithoutArchivals,
          rubrics);
      }

       return wikiTreeCache;
    }
  }
}
