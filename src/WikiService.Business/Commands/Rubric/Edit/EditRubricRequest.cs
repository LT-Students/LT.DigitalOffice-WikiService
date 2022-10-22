using System;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric
{
  public class EditRubricRequest
  {
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public bool IsActive { get; set; }
  }
}
