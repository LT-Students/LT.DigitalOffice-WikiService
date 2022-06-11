using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Models;
using LT.DigitalOffice.WikiService.Models.Dto.Responses.Rubric;
using System.Collections.Generic;

namespace LT.DigitalOffice.WikiService.Mappers.Responses.Interfaces
{
  [AutoInject]
  public interface IRubricResponseMapper
  {
    RubricResponse Map(
      DbRubric dbRubric); 
  }
}
