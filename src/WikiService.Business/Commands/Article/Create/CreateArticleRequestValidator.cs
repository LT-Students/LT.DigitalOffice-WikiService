using FluentValidation;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Validation.Article
{
  public class CreateArticleRequestValidator : AbstractValidator<CreateArticleRequest>
  {
    private readonly IDataProvider _provider;

    private async Task<bool> DoesExistAsync(Guid rubricId)
    {
      return await _provider.Rubrics.AnyAsync(x => x.Id == rubricId && x.IsActive);
    }

    public CreateArticleRequestValidator(
      IDataProvider provider)
    {
      _provider = provider;

      RuleFor(article => article.RubricId)
        .MustAsync(async (rubricId, _) => await DoesExistAsync(rubricId))
        .WithMessage("This rubric does not exist or is not active.");

      RuleFor(article => article.Name)
        .MaximumLength(210)
        .WithMessage("Article name is too long.");
    }
  }
}