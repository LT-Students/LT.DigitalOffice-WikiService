using FluentValidation;
using LT.DigitalOffice.WikiService.Data.Interfaces;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric.Create
{
  public class CreateRubricRequestValidator : AbstractValidator<CreateRubricRequest>
  {
    public CreateRubricRequestValidator(
      IRubricRepository rubricRepository)
    {
      CascadeMode = CascadeMode.Stop;

      RuleFor(rubric => rubric.Name)
        .MaximumLength(101)
        .WithMessage("Rubric name is too long.");

      When(rubric => rubric.ParentId.HasValue, () =>
      {
        RuleFor(request => request.ParentId)
          .NotEmpty()
          .WithMessage("ParentId must not be empty.")
          .MustAsync(async (parentId, _) => await rubricRepository.DoesExistAsync(parentId.Value))
          .WithMessage("This rubric id doesn't exist.");
      });

      RuleFor(request => request)
        .MustAsync(async (request, _) => !await rubricRepository.DoesRubricNameExistAsync(request.ParentId, request.Name))
        .WithMessage("This rubric name already exists.");
    }
  }
}
