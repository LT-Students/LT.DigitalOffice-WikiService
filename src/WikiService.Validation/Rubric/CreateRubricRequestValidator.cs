using FluentValidation;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Models.Dto.Requests;
using LT.DigitalOffice.WikiService.Validation.Rubric.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Validation.Rubric
{
  public class CreateRubricRequestValidator : AbstractValidator<CreateRubricRequest>, ICreateRubricRequestValidator
  {
    public CreateRubricRequestValidator(
      IRubricRepository rubricRepository)
    {
      CascadeMode = CascadeMode.Stop;

      RuleFor(rubric => rubric.Name.Trim())
        .NotEmpty().WithMessage("Rubric name must not be empty.")
        .MaximumLength(150).WithMessage("Rubric name is too long.");

      When(rubric => rubric.ParentId.HasValue, () =>
      {
        RuleFor(request => request.ParentId)
          .MustAsync(async (parentId, _) => await rubricRepository.DoesExistAsync(parentId.Value))
          .WithMessage(project => "Invalid ParentRubric id.");

        RuleFor(request => request)
          .MustAsync(async (request, _) => await rubricRepository.DoesSubrubricNameExistAsync(request.ParentId.Value, request.Name));
      });

      When(rubric => !rubric.ParentId.HasValue, () =>
      {
        RuleFor(request => request.Name)
          .MustAsync(async (name, _) => await rubricRepository.DoesRubricNameExistAsync(name));
      });
    }
  }
}
