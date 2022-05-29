using LT.DigitalOffice.WikiService.Mappers.Models.Interfaces;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Models;
using System.Linq;

namespace LT.DigitalOffice.WikiService.Mappers.Models
{
  public class RubricInfoMapper : IRubricInfoMapper
  {
    public RubricInfo Map(DbRubric dbRubric)
    {
      if (dbRubric is null)
      {
        return null;
      }

      return new RubricInfo
      {
        Id = dbRubric.Id,
        Name = dbRubric.Name,
        ParentId = dbRubric.ParentId,
        IsActive = dbRubric.IsActive,
        HasChild = dbRubric.HasChild
      };
    }
  }
}
