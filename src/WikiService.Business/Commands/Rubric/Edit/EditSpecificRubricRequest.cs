using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric.Edit
{
  public record EditSpecificRubricRequest : IRequest<bool>
  {
    public Guid Id { get; set; }
    public JsonPatchDocument<EditRubricRequest> Request { get; set; }
  }
}
