using LT.DigitalOffice.WikiService.Mappers.Db.Interfaces;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests;

namespace LT.DigitalOffice.WikiService.Mappers.Db
{
  public class RubricMapper : IRubricMapper
  {
    public Rubric Map(DbRubric dbRubric)
    {
      if (dbRubric is null)
      {
        return null;
      }

      return new Rubric
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
