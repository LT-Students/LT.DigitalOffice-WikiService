using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.WikiService.Models.Dto.Models
{
  public record RubricData
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public Guid? ParentId { get; set; }
    public List<string> ArticlesNames { get; set; }
    public List<RubricData> Children { get; set; }
  }
}
