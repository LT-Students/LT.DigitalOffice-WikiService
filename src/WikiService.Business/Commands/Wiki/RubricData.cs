using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.WikiService.Business.Commands.Wiki
{
  public record RubricData
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public Guid? ParentId { get; set; }
    public List<ArticleData> Articles { get; set; }
    public List<RubricData> Children { get; set; }
  }
}
