using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Responses.Rubric;

namespace LT.DigitalOffice.WikiService.Mappers.Responses.Interfaces
{
  [AutoInject]
  public interface IRubricResponseMapper
  {
    RubricResponse Map(DbRubric dbRubric, bool includeSubrubrics); 
  }
}
