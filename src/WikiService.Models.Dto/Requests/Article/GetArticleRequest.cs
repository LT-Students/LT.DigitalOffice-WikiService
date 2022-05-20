using System;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.WikiService.Models.Dto.Requests.Article
{
  public record GetArticleRequest
  {
    [FromQuery(Name = "articleId")]
    public Guid? ArticleId { get; set; }
  }
}
