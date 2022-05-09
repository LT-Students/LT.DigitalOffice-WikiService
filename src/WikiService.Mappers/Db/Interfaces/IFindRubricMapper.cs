using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests;

namespace LT.DigitalOffice.WikiService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IFindRubricMapper
  {
    FindRubricRequest Map(DbRubric dbRubric);
  }
}
