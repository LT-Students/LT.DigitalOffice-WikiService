using LT.DigitalOffice.WikiService.Models.Dto.Models;
using System.Collections.Generic;

namespace LT.DigitalOffice.WikiService.Models.Dto.Responses.Rubric
{
  public record RubricResponse
  {
      public RubricInfo Rubric { get; set; }
      public IEnumerable<RubricInfo> SubRubrics { get; set; }
      public IEnumerable<ArticleInfo> Articles { get; set; }
    }
  }
