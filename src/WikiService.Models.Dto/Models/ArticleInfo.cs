using System;

namespace LT.DigitalOffice.WikiService.Models.Dto.Models
{
  public record ArticleInfo
  {
    public Guid? ArticleId { get; set; }
    public string ArticleName { get; set; }
  }
}
