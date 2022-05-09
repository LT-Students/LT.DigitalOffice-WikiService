using Microsoft.AspNetCore.Mvc;
using LT.DigitalOffice.Kernel.Requests;

namespace LT.DigitalOffice.WikiService.Models.Dto.Requests.Filters
{
  public record FindRubricFilter : BaseFindFilter
  {
    [FromQuery(Name = "nameIncludeSubstring")]
    public string NameIncludeSubstring { get; set; }

    [FromQuery(Name = "isAscendingSort")]
    public bool? IsAscendingSort { get; set; }
  }
}
