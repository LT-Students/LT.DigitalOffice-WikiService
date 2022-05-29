using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Responses;
using System.Collections.Generic;

namespace LT.DigitalOffice.WikiService.Mappers.Responses.Interfaces
{
  [AutoInject]
  public interface IRubricResponseMapper
  {
    RubricResponse Map(
      DbRubric dbRubric, 
      List<DbRubric> childRubrics);
  }
}
