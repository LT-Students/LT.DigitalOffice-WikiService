using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.WikiService.Models.Dto.Responses.Article
{
  public record ArticleResponse
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public bool IsActive { get; set; }
    public Guid RubricId { get; set; }
    public List<Guid> Files { get; set; }
  }
}
