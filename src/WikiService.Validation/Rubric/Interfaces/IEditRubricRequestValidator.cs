using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric;
using Microsoft.AspNetCore.JsonPatch;
using System;

namespace LT.DigitalOffice.WikiService.Validation.Rubric.Interfaces
{
  [AutoInject]
  public interface IEditRubricRequestValidator : IValidator<(Guid,JsonPatchDocument<EditRubricRequest>)>
  {
  }
}
