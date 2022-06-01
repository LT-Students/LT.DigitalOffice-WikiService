using System;

namespace LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric
{
  public record EditRubricRequest
  {
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public bool IsActive { get; set; }
  }
}
