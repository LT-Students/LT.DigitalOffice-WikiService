using LT.DigitalOffice.WikiService.Mappers.Models.Interfaces;
using LT.DigitalOffice.WikiService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Models;
using LT.DigitalOffice.WikiService.Models.Dto.Responses.Rubric;
using System.Linq;

namespace LT.DigitalOffice.WikiService.Mappers.Responses
{
  public class RubricResponseMapper : IRubricResponseMapper
  {
    private readonly IRubricInfoMapper _rubricInfoMapper;
    private readonly IArticleInfoMapper _articleInfoMapper;

    public RubricResponseMapper(
      IRubricInfoMapper rubricInfoMapper,
      IArticleInfoMapper articleInfoMapper)
    {
      _rubricInfoMapper = rubricInfoMapper;
      _articleInfoMapper = articleInfoMapper;
    }

    public RubricResponse Map(
      DbRubric dbRubric, bool includeSubRubrics)
    {
      if (dbRubric is null)
      {
        return null;
      }

      return new RubricResponse
      {
        Rubric = new RubricInfo
        {
          Id = dbRubric.Id,
          Name = dbRubric.Name,
          ParentId = dbRubric.ParentId,
          IsActive = dbRubric.IsActive,
          HasChild = dbRubric.SubRubrics.Any()
            ? true 
            : false
        },

        SubRubrics = includeSubRubrics
          ? dbRubric.SubRubrics?
              .Select(x => _rubricInfoMapper.Map(x))
          : null,

        Articles = dbRubric.Articles?
          .Select(x => _articleInfoMapper.Map(x))
      };
    }
  }
}
