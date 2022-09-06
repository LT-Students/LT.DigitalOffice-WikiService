using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LT.DigitalOffice.WikiService.Business.Commands.Wiki
{
  public record GetWikiFilter : IRequest<List<RubricData>>
  {
    [FromQuery(Name = "includedeactivated")]
    public bool includeDeactivated { get; set; } = false;
  }
}
