using LT.DigitalOffice.WikiService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Models;
using LT.DigitalOffice.WikiService.Models.Dto.Responses.Rubric;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.WikiService.Mappers.Responses
{
  public class RubricResponseMapper : IRubricResponseMapper
  {
    public RubricResponse Map(
      DbRubric dbRubric,
      List<DbRubric> subRubrics)
    {

      if (dbRubric == null)
      {
        return null;
      }

      return new RubricResponse
      {
        Id = dbRubric.Id,
        Name = dbRubric.Name,
        ParentId = dbRubric.ParentId,
        IsActive = dbRubric.IsActive,

        SubRubrics = subRubrics
        ?.Select(x => new SubRubricInfo
        {
          SubRubricId = x.Id,
          SubRubricName = x.Name
        }),

        Articles = dbRubric.Articles
          ?.Select(c => new ArticleInfo
          {
            ArticleId = c.Id,
            ArticleName = c.Name
          }),
      };
    }
  }
}
