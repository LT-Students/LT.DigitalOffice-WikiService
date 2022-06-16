using System;

namespace LT.DigitalOffice.WikiService.Models.Dto.Requests.Article
{
  public record EditArticleRequest(string Name, string Content, bool IsActive, Guid RubricId);
}
