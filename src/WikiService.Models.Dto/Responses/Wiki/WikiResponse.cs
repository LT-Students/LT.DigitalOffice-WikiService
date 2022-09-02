using System;

namespace LT.DigitalOffice.WikiService.Models.Dto.Responses.Wiki
{
  public record WikiResponse
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
  }
}
