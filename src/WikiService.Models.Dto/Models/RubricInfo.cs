using System;

namespace LT.DigitalOffice.WikiService.Models.Dto.Models
{
  public record RubricInfo
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public bool IsActive { get; set; }
    public bool HasChild { get; set; }
  }
}
