using System;

namespace LT.DigitalOffice.WikiService.Models.Dto.Requests.Article
{
  public record EditArticleRequest
  {
    public string Name { get; set; }
    public string Content { get; set; }
    public bool IsActive { get; set; }
    public Guid RubricId { get; set; }
  }
}
