using Microsoft.AspNetCore.Mvc;
using LT.DigitalOffice.Kernel.Requests;

namespace LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric.Filters
{
  public record FindRubricFilter : BaseFindFilter
  {
    [FromQuery(Name = "nameIncludeSubstring")]
    public string NameIncludeSubstring { get; set; }

    [FromQuery(Name = "isAscendingSort")]
    public bool? IsAscendingSort { get; set; }

    [FromQuery(Name = "isActive")]
    public bool? IsActive { get; set; }
  }
}
