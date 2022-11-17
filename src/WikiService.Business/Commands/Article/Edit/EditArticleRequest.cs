using System;

namespace LT.DigitalOffice.WikiService.Business.Commands.Article.Edit
{
  public class EditArticleRequest
  {
    public string Name { get; set; }
    public string Content { get; set; }
    public bool IsActive { get; set; }
    public Guid RubricId { get; set; }
    public int Position { get; set; }
  }
}
