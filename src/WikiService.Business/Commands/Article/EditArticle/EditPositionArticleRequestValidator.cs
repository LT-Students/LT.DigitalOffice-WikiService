using FluentValidation;
using LT.DigitalOffice.WikiService.Data.Provider;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Article.EditPosition
{
  public class EditPositionArticleRequestValidator : AbstractValidator<EditPositionArticleRequest>
  {
    private readonly IDataProvider _provider;

    private async Task<bool> DoesExistAsync(EditPositionArticleRequest request)
    {
      return await _provider.Articles.AnyAsync(x => x.Id == request.Id && x.RubricId == request.RubricId);
    }

    private async Task<int> CountChildrenAsync(EditPositionArticleRequest request)
    {
      return await _provider.Articles.CountAsync(x => x.RubricId == request.RubricId);
    }

    public EditPositionArticleRequestValidator(
      IDataProvider provider)
    {
      _provider = provider;
      CascadeMode = CascadeMode.Stop;

      RuleFor(request => request)
        .MustAsync(async (request, _) => await DoesExistAsync(request))
        .WithMessage("Article doesn't exist.")
        .Must(request => request.Position > 0)
        .WithMessage("Position must be greater than 0.")
        .MustAsync(async (request, _) => request.Position <= await CountChildrenAsync(request))
        .WithMessage("Position is too large.");
    }
  }
}
