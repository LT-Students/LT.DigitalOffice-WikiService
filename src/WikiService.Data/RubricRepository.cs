using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Data
{
  public class RubricRepository : IRubricRepository
  {
    private readonly IDataProvider _provider;

    public RubricRepository(
      IDataProvider provider)
    {
      _provider = provider;
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

    public async Task<bool> DoesRubricIdExistAsync(Guid rubricId)
    {
      return await _provider.Rubrics.AnyAsync(x => x.Id == rubricId);
    }

    public async Task<bool> DoesRubricNameExistAsync(Guid? rubricParentId, string nameRubric)
    {
      return await _provider.Rubrics.AnyAsync(p => p.ParentId.Value == rubricParentId && p.Name == nameRubric);
    }
  }
}