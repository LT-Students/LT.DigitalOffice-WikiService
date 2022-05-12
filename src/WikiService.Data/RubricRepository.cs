using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Filters;

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
        dbRubric = filter.IsActive.Value
          ? dbRubric.Where(rubric => rubric.IsActive == true)
          : dbRubric.Where(rubric => rubric.IsActive == false);
      }

      else
      {
        dbRubric = dbRubric.OrderByDescending(rubric => rubric.CreatedAtUtc);
      }

      foreach(var rubric in dbRubric)
      {
        foreach (var id in _provider.Rubrics.AsQueryable())
        {
          if (id.ParentId == rubric.Id)
          {
            rubric.HasChild = true;
            break;
          }
        }         
      }

      return dbRubric;
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
  }
}