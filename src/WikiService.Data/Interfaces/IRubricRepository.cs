using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric.Filters;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Data.Interfaces
{
  [AutoInject]
  public interface IRubricRepository
  {
    Task<Guid?> CreateAsync(DbRubric dbRubric);

    Task<DbRubric> GetAsync(Guid rubricId);

    Task<bool> EditAsync(DbRubric dbRubric, JsonPatchDocument<DbRubric> request);

    Task<bool> DoesExistAsync(Guid rubricId);

    Task<bool> DoesRubricNameExistAsync(Guid? rubricParentId, string nameRubric);

    Task<(List<DbRubric> dbRubric, int totalCount)> FindAsync(FindRubricFilter filter);
  }
}
