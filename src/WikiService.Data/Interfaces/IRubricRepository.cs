using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Data.Interfaces
{
  [AutoInject]
  public interface IRubricRepository
  {
    Task<(List<DbRubric> dbRubric, int totalCount)> FindAsync(FindRubricFilter filter);
  }
}
