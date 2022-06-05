using LT.DigitalOffice.Kernel.Requests;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric.Filters
{
  public record GetRubricFilter
  {
    [FromQuery(Name = "rubricId")]
    public Guid RubricId { get; set; }

    [FromQuery(Name = "includeSubRubrics")]
    public bool IncludeSubRubrics { get; set; } = false;

    [FromQuery(Name = "includeArticles")]
    public bool IncludeArticles { get; set; } = false;
  }
}
