using LT.DigitalOffice.WikiService.Models.Dto.Models;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.WikiService.Models.Dto.Requests.Article
{
  public record CreateArticleRequest
  {
    public string Name { get; set; }
    public string Content { get; set; }
    public bool IsActive { get; set; }
    public Guid RubricId { get; set; }
    public List<File> Files { get; set; } 
  }
}


