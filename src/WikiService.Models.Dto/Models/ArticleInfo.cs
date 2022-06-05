using System;

namespace LT.DigitalOffice.WikiService.Models.Dto.Models
{
  public record ArticleInfo
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
  }
}
