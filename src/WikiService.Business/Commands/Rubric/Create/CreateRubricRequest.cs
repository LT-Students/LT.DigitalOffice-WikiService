using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric.Create
{
  public record CreateRubricRequest : IRequest<Guid?>
  {
    [Required]
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
  }
}
