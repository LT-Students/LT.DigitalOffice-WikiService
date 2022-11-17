using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.WikiService.Business.Commands.Wiki;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric.Create
{
  public class CreateRubricHandler : IRequestHandler<CreateRubricRequest, Guid?>
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDataProvider _provider;
    private readonly IMemoryCache _cache;

    private async Task<Guid?> CreateAsync(DbRubric dbRubric, CancellationToken ct)
    {
      if (dbRubric is null)
      {
        return null;
      }

      int position = await _provider.Rubrics
        .Where(x => x.ParentId == dbRubric.ParentId)
        .CountAsync();
      dbRubric.Position = position + 1;
      
      await _provider.Rubrics.AddAsync(dbRubric, ct);
      await _provider.SaveAsync();

      return dbRubric.Id;
    }

    private DbRubric Map(CreateRubricRequest request)
    {
      if (request is null)
      {
        return null;
      }

      return new DbRubric
      {
        Id = Guid.NewGuid(),
        Name = request.Name.Trim(),
        ParentId = request.ParentId,
        IsActive = true,
        CreatedAtUtc = DateTime.UtcNow,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId()
      };
    }

    public CreateRubricHandler(
      IHttpContextAccessor httpContextAccessor,
      IDataProvider provider,
      IMemoryCache cache)
    {
      _httpContextAccessor = httpContextAccessor;
      _provider = provider;
      _cache = cache;
    }

    public async Task<Guid?> Handle(CreateRubricRequest request, CancellationToken ct)
    {
      _cache.Remove(CacheKeys.WikiTreeWithDeactivated);
      _cache.Remove(CacheKeys.WikiTreeWithoutDeactivated);

      return await CreateAsync(Map(request), ct);
    }
  }
}
