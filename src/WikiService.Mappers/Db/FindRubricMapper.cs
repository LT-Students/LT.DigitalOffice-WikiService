using LT.DigitalOffice.WikiService.Mappers.Db.Interfaces;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests;

namespace LT.DigitalOffice.WikiService.Mappers.Db
{
  public class FindRubricMapper : IFindRubricMapper
  {
    public FindRubricRequest Map(DbRubric dbRubric)
    {
      if (dbRubric is null)
      {
        return null;
      }

      return new FindRubricRequest
      {
        Id = dbRubric.Id,
        Name = dbRubric.Name,
        ParentId = dbRubric.ParentId,
        ChildCount = dbRubric.ChildCount,
        IsActive = dbRubric.IsActive
      };
    }
  }
}
