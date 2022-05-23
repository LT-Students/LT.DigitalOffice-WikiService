using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Validation.Rubric.Interfaces
{
  public interface IEditRubricRequestValidator : IValidator<(Guid,JsonPatchDocument<EditRubricRequest>)>
  {
  }
}
