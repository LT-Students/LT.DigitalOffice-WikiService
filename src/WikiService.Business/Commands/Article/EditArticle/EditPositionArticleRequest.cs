using MediatR;
using System;

namespace LT.DigitalOffice.WikiService.Business.Commands.Article.EditPosition
{
  public record EditPositionArticleRequest : IRequest<bool>
  {
    public Guid Id { get; set; }
    public Guid RubricId { get; set; }
    public int Position { get; set; }
  }
}
