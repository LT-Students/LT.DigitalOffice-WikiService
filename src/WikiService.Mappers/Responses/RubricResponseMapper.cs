using LT.DigitalOffice.WikiService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Models;
using LT.DigitalOffice.WikiService.Models.Dto.Responses.Rubric;
using System.Linq;

namespace LT.DigitalOffice.WikiService.Mappers.Responses
{
  public class RubricResponseMapper : IRubricResponseMapper
  {
    public RubricResponse Map(
      DbRubric dbRubric)
    {

      if (dbRubric is null)
      {
        return null;
      }

      return new RubricResponse
      {
        Id = dbRubric.Id,
        Name = dbRubric.Name,
        ParentId = dbRubric.ParentId,
        IsActive = dbRubric.IsActive,

        SubRubrics = dbRubric.ParentIds
          ?.Select(x => new RubricInfo
          {
            Id = x.Id,
            Name = x.Name,
            ParentId = x.ParentId,
            IsActive = x.IsActive,
            HasChild = x.HasChild
      }),

        Articles = dbRubric.Articles
          ?.Select(c => new ArticleInfo
          {
            Id = c.Id,
            Name = c.Name
          }),
      };
    }
  }
}
