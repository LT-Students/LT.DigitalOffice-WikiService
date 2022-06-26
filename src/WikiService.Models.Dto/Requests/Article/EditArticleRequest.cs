using System;

namespace LT.DigitalOffice.WikiService.Models.Dto.Requests.Article
{
  public class EditArticleRequest
  {
    public string Name;
    public string Content;
    public bool IsActive;
    public Guid RubricId;
  }
}
