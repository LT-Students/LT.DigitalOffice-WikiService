using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.WikiService.Models.Dto.Responses.Wiki
{
  public record WikiTreeResponse
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public List<string> ArticlesNames { get; set; }
    public List<WikiTreeResponse> Children { get; set; }
  }
}
