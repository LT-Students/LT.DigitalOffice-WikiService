using System;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric
{
  public record EditRubricRequest
  {
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public bool IsActive { get; set; }
    public int Position { get; set; }
  }
}
