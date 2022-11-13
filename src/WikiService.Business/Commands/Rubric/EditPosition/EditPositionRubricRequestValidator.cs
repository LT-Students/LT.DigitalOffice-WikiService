using FluentValidation;
using LT.DigitalOffice.WikiService.Data.Provider;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric.EditPosition
{
  public class EditPositionArticleRequestValidator : AbstractValidator<EditPositionRubricRequest>
  {
    private readonly IDataProvider _provider;

    private async Task<bool> DoesExistAsync(EditPositionRubricRequest request)
    {
      return await _provider.Rubrics.AnyAsync(x => x.Id == request.Id && x.ParentId == request.ParentRubricId);
    }

    private async Task<int> CountChildrenAsync(EditPositionRubricRequest request)
    {
      return await _provider.Rubrics.CountAsync(x => x.ParentId == request.ParentRubricId);
    }

    public EditPositionArticleRequestValidator(
      IDataProvider provider)
    {
      _provider = provider;
      CascadeMode = CascadeMode.Stop;

      RuleFor(request => request)
        .MustAsync(async (request, _) => await DoesExistAsync(request))
        .WithMessage("Rubric doesn't exist.")
        .Must(request => request.Position > 0)
        .WithMessage("Position must be greater than 0.")
        .MustAsync(async (request, _) => request.Position <= await CountChildrenAsync(request))
        .WithMessage("Position is too large.");
    }
  }
}
