using System;

namespace LT.DigitalOffice.WikiService.Business.Commands.Article.Edit
{
  public record EditArticleRequest
  {
    public string Name;
    public string Content;
    public bool IsActive;
    public Guid RubricId;
  }
}
