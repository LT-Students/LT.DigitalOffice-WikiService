using System;

namespace LT.DigitalOffice.WikiService.Models.Dto.Requests
{
  public record CreateRubricRequest
  {
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
  }
}
