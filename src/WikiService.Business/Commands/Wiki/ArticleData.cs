using System;

namespace LT.DigitalOffice.WikiService.Business.Commands.Wiki
{
  public record ArticleData
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool isActive { get; set; }
  }
}
