using LT.DigitalOffice.Kernel.Attributes;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Data.Interfaces
{
  [AutoInject]
  public interface IRubricRepository
  {
    Task<bool> DoesExistAsync(Guid rubricId);

    Task<bool> DoesRubricNameExistAsync(Guid? rubricParentId, string nameRubric);
  }
}
