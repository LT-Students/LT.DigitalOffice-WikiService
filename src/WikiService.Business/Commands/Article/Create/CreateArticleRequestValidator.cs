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

    private async Task<bool> DoesSameNameExistAsync(Guid rubricId, string articleName)
    {
      return await _provider.Articles.AnyAsync(a => a.RubricId == rubricId && a.Name == articleName);
    }

    private async Task<bool> DoesExistAsync(Guid rubricId)
    {
      return await _provider.Rubrics.AnyAsync(x => x.Id == rubricId);
    }

    public CreateArticleRequestValidator(
      IDataProvider provider)
    {
      _provider = provider;

      RuleFor(article => article.RubricId)
        .MustAsync(async (rubricId, _) => await DoesExistAsync(rubricId))
        .WithMessage("This rubric id does not exist.");

      RuleFor(article => article.Name)
        .MaximumLength(210)
        .WithMessage("Article name is too long.");

      RuleFor(article => article)
        .MustAsync(async (article, _) => !await DoesSameNameExistAsync(article.RubricId, article.Name))
        .WithMessage("This article name already exists.");
    }
  }
}