using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric;

namespace LT.DigitalOffice.WikiService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbRubricMapper
  {
    DbRubric Map(CreateRubricRequest request);
  }
}
