using FluentValidation;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Models.Dto.Requests;
using LT.DigitalOffice.WikiService.Validation.Rubric.Interfaces;

namespace LT.DigitalOffice.WikiService.Validation.Rubric
{
  public class CreateRubricRequestValidator : AbstractValidator<CreateRubricRequest>, ICreateRubricRequestValidator
  {
    public CreateRubricRequestValidator(
      IRubricRepository rubricRepository)
    {
      CascadeMode = CascadeMode.Stop;

      RuleFor(rubric => rubric.Name.Trim())
        .MaximumLength(150)
        .WithMessage("Rubric name is too long.");

      When(rubric => rubric.ParentId.HasValue, () =>
      {
        RuleFor(request => request.ParentId)
          .NotEmpty()
          .WithMessage("ParentId must not be empty.")
          .MustAsync(async (parentId, _) => await rubricRepository.DoesRubricIdExistAsync(parentId.Value))
          .WithMessage("This rubric id does not exist.")
          .DependentRules(() =>
          {
            RuleFor(request => request)
           .MustAsync(async (request, _) => !await rubricRepository.DoesRubricNameExistAsync(request.ParentId.Value, request.Name))
           .WithMessage("This subrubric name already exists.");
          });
      });

      When(rubric => !rubric.ParentId.HasValue, () =>
      {
        RuleFor(request => request.Name)
          .MustAsync(async (name, _) => !await rubricRepository.DoesRubricNameExistAsync(null, name))
          .WithMessage("This rubric name already exists.");
      });
    }
  }
}
