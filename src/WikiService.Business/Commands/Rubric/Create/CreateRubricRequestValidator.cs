using FluentValidation;
using LT.DigitalOffice.WikiService.Data.Provider;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric.Create
{
  public class CreateRubricRequestValidator : AbstractValidator<CreateRubricRequest>
  {
    private readonly IDataProvider _provider;

    private async Task<bool> DoesExistAsync(Guid rubricId)
    {
      return await _provider.Rubrics.AnyAsync(x => x.Id == rubricId && x.IsActive);
    }

    private async Task<bool> DoesRubricNameExistAsync(Guid? rubricParentId, string nameRubric)
    {
      return await _provider.Rubrics.AnyAsync(p => p.ParentId == rubricParentId && p.Name.ToLower() == nameRubric.ToLower());
    }

    public CreateRubricRequestValidator(
      IDataProvider provider)
    {
      _provider = provider;
      CascadeMode = CascadeMode.Stop;

      RuleFor(rubric => rubric.Name)
        .MaximumLength(101)
        .WithMessage("Rubric name is too long.");

      When(rubric => rubric.ParentId.HasValue, () =>
      {
        RuleFor(request => request.ParentId)
          .NotEmpty()
          .WithMessage("ParentId must not be empty.")
          .MustAsync(async (parentId, _) => await DoesExistAsync(parentId.Value))
          .WithMessage("This rubric id doesn't exist.");
      });

      RuleFor(request => request)
        .MustAsync(async (request, _) => !await DoesRubricNameExistAsync(request.ParentId, request.Name))
        .WithMessage("This rubric name already exists.");
    }
  }
}
