using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Db;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Data.Interfaces
{
  [AutoInject]
  public interface IRubricRepository
  {
    Task<Guid?> CreateAsync(DbRubric dbRubric);

    Task<bool> DoesExistAsync(Guid rubricId);

    Task<bool> DoesRubricNameExistAsync(Guid? rubricParentId, string nameRubric);
  }
}
