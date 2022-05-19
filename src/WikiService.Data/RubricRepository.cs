using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric.Filters;

namespace LT.DigitalOffice.WikiService.Data
{
  public class RubricRepository : IRubricRepository
  {
    private readonly IDataProvider _provider;

    private IQueryable<DbRubric> CreateFindPredicates(
      FindRubricFilter filter,
      IQueryable<DbRubric> dbRubric)
    {
      if (!string.IsNullOrEmpty(filter.NameIncludeSubstring))
      {
        dbRubric = dbRubric.Where(
          rubric =>
            rubric.Name.Contains(filter.NameIncludeSubstring.ToLower()));
      }

      if (filter.IsAscendingSort.HasValue)
      {
        dbRubric = filter.IsAscendingSort.Value
          ? dbRubric.OrderBy(rubric => rubric.Name)
          : dbRubric.OrderByDescending(rubric => rubric.Name);
      }

      if (filter.IsActive.HasValue)
      {
        dbRubric = dbRubric.Where(rubric => rubric.IsActive == filter.IsActive.Value);
      }
      else
      {
        dbRubric = dbRubric.OrderByDescending(rubric => rubric.CreatedAtUtc);
      }

      return FindRubricChild(ref dbRubric);
    }

    public RubricRepository(
      IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task<(List<DbRubric> dbRubric, int totalCount)> FindAsync(FindRubricFilter filter)
    {
      if (filter is null)
      {
        return (null, default);
      }

      IQueryable<DbRubric> dbRubric = CreateFindPredicates(
        filter,
        _provider.Rubrics.AsQueryable().Where(rubric => rubric.ParentId == null));

      return (
        await dbRubric.Skip(filter.SkipCount).Take(filter.TakeCount).ToListAsync(),
        await dbRubric.CountAsync());
    }


    private IQueryable<DbRubric> FindRubricChild(ref IQueryable<DbRubric> dbRubric)
    {
      foreach (DbRubric topRubric in dbRubric.ToList())
      {
        foreach (DbRubric rubric in _provider.Rubrics.AsEnumerable())
        {
          if (rubric.ParentId == topRubric.Id)
          {
            topRubric.HasChild = true;
            break;
          }
        }

        if (!topRubric.HasChild)
        {
          foreach (DbArticle article in _provider.Articles.AsEnumerable())
          {
            if (article.RubricId == topRubric.Id)
            {
              topRubric.HasChild = true;
              break;
            }
          }
        }
      }

      return dbRubric;
    }
  }
}