using MediatR;
using System;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric.EditPosition
{
  public record EditPositionRubricRequest : IRequest<bool>
  {
    public Guid Id { get; set; }
    public Guid? ParentRubricId { get; set; }
    public int Position { get; set; }
  }
}
