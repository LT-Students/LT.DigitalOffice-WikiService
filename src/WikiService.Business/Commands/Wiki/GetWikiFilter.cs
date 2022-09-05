using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Models.Dto.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LT.DigitalOffice.WikiService.Business.Commands.Wiki
{
  public record GetWikiFilter : IRequest<List<RubricData>>
  {
    [FromQuery(Name = "includearchivals")]
    public bool IncludeArchivals { get; set; } = false;
  }
}
