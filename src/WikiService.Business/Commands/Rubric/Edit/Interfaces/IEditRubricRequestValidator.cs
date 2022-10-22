using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Db;
using Microsoft.AspNetCore.JsonPatch;
using System;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric.Interfaces
{
  [AutoInject]
  public interface IEditRubricRequestValidator : IValidator<(DbRubric,JsonPatchDocument<EditRubricRequest>)>
  {
  }
}
