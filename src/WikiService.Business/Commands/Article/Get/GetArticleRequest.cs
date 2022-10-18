using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.WikiService.Business.Commands.Article.Get
{
  public record GetArticleRequest : IRequest<ArticleResponse>
  {
    [Required]
    public Guid Id { get; set; }
  }
}