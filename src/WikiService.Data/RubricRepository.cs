using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Data.Provider;
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

    public async Task<bool> DoesExistAsync(Guid rubricId)
    {
      return await _provider.Rubrics.AnyAsync(x => x.Id == rubricId);
    }

    public async Task<bool> DoesRubricNameExistAsync(Guid? rubricParentId, string nameRubric)
    {
      return await _provider.Rubrics.AnyAsync(p => p.ParentId == rubricParentId && p.Name.ToLower() == nameRubric.ToLower());
    }
  }
}