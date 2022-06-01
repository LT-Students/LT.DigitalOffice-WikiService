using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.WikiService.Mappers.PatchDocument.Interfaces
{
  [AutoInject]
  public interface IPatchDbRubricMapper
  {
    JsonPatchDocument<DbRubric> Map(JsonPatchDocument<EditRubricRequest> request);
  }
}
