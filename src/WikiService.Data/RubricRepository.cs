using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

      foreach (DbRubric topRubric in dbRubric)
      {
        foreach (DbRubric rubric in _provider.Rubrics.AsQueryable())
        {
          if (rubric.ParentId == topRubric.Id)
          {
            topRubric.HasChild = true;
            break;
          }
        }

        if (!topRubric.HasChild)
        {
          foreach (DbArticle article in _provider.Articles.AsQueryable())
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

    public async Task<Guid?> CreateAsync(DbRubric dbRubric)
    {
      if (dbRubric is null)
      {
        return null;
      }

      _provider.Rubrics.Add(dbRubric);
      await _provider.SaveAsync();

      return dbRubric.Id;
    }

    public async Task<bool> DoesExistAsync(Guid rubricId)
    {
      return await _provider.Rubrics.AnyAsync(x => x.Id == rubricId);
    }

    public async Task<bool> DoesRubricNameExistAsync(Guid? rubricParentId, string nameRubric)
    {
      return await _provider.Rubrics.AnyAsync(p => p.ParentId == rubricParentId && p.Name == nameRubric);
    }

    public async Task<(DbRubric, bool)> GetAsync(GetRubricFilter filter)
    {
      IQueryable<DbRubric> dbRubrics = _provider.Rubrics.AsQueryable();

      dbRubrics = _provider.Rubrics.Include(x => x.SubRubrics);

      await dbRubrics.ToListAsync();

      if (filter.IncludeSubRubrics)
      {
        foreach (DbRubric subRubric in dbRubrics)
        {
          if (subRubric.SubRubrics.Any())
          {
            subRubric.HasChild = true;
          }
        }
      }

      if (filter.IncludeArticles)
      {
        dbRubrics = _provider.Rubrics.Include(x => x.Articles);
      }

      return (
        await dbRubrics.FirstOrDefaultAsync(x => x.Id == filter.RubricId),
        filter.IncludeSubRubrics);
    }
  }
}