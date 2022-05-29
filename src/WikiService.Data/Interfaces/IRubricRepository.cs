using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Data.Interfaces
{
  [AutoInject]
  public interface IRubricRepository
  {
    Task<Guid?> CreateAsync(DbRubric dbRubric);

    Task<bool> DoesExistAsync(Guid rubricId);

    Task<bool> DoesRubricNameExistAsync(Guid? rubricParentId, string nameRubric);

    Task<(List<DbRubric> dbRubric, int totalCount)> FindAsync(FindRubricFilter filter);

    Task<List<DbRubric>> GetSubRubricsAsync(GetRubricFilter filter);

    Task<DbRubric> GetAsync(GetRubricFilter filter);
  }
}
