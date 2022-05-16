using LT.DigitalOffice.WikiService.Models.Dto.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.WikiService.Models.Dto.Requests.Article
{
  public record CreateArticleRequest
  {
    [Required]
    public string Name { get; set; }
    [Required]
    public string Content { get; set; }
    public Guid RubricId { get; set; }
    public List<CreateFileRequest> Files { get; set; }
  }
}