using LT.DigitalOffice.WikiService.Business.Commands.Wiki;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Article.EditPosition
{
  public class EditPositionArticleHandler : IRequestHandler<EditPositionArticleRequest, bool>
  {
    private readonly IDataProvider _provider;
    private readonly IMemoryCache _cache;

    private Task<ValueTuple<int, Guid>[]> GetPositionsAsync(Guid rubricId)
    {
      return _provider.Articles
        .Where(x => x.RubricId == rubricId)
        .Select(x => new ValueTuple<int, Guid>(x.Position, x.Id))
        .ToArrayAsync();
    }

    private async Task<bool> EditPositionsAsync(
      ValueTuple<int, Guid>[] positions,
      Guid rubricId)
    {
      List<DbArticle> articles = await _provider.Articles.Where(x => x.RubricId == rubricId).ToListAsync();

      foreach (DbArticle atricle in articles)
      {
        atricle.Position = positions.FirstOrDefault(x => x.Item2 == atricle.Id).Item1;
      }

      await _provider.SaveAsync();

      return true;
    }

    public EditPositionArticleHandler(
      IDataProvider provider,
      IMemoryCache cache)
    {
      _provider = provider;
      _cache = cache;
    }

    public async Task<bool> Handle(EditPositionArticleRequest request, CancellationToken ct)
    {
      ValueTuple<int, Guid>[] positions = await GetPositionsAsync(request.RubricId);

      Array.Sort(positions);

      for (int i = 0; i < positions.Length; i++)
      {
        if (positions[i].Item2 == request.Id)
        {
          if (positions[i].Item1 < request.Position)
          {
            int j = i + 1;
            while (j < positions.Length && positions[j].Item1 <= request.Position)
            {
              positions[j].Item1--;
              j++;
            }
          }
          else
          {
            int j = i - 1;
            while (j >= 0 && positions[j].Item1 >= request.Position)
            {
              positions[j].Item1++;
              j--;
            }
          }

          positions[i].Item1 = request.Position;
          break;
        }
      }

      _cache.Remove(CacheKeys.WikiTreeWithDeactivated);
      _cache.Remove(CacheKeys.WikiTreeWithoutDeactivated);

      return await EditPositionsAsync(positions, request.RubricId);
    }
  }
}
