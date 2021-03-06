using FluentValidation;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;
using LT.DigitalOffice.WikiService.Validation.Article.Interfaces;

namespace LT.DigitalOffice.WikiService.Validation.Article
{
  public class CreateArticleRequestValidator : AbstractValidator<CreateArticleRequest>, ICreateArticleRequestValidator
  {
    public CreateArticleRequestValidator(
      IRubricRepository rubricRepository,
      IArticleRepository articleRepository)
    {
      CascadeMode = CascadeMode.Stop;

      RuleFor(article => article.RubricId)
        .MustAsync(async (rubricId, _) => await rubricRepository.DoesExistAsync(rubricId))
        .WithMessage("This rubric id does not exist.");

      RuleFor(article => article.Name)
        .MaximumLength(150)
        .WithMessage("Article name is too long.");

      RuleFor(article => article)
        .MustAsync(async (article, _) => !await articleRepository.DoesSameNameExistAsync(article.RubricId, article.Name))
        .WithMessage("This article name already exists.");
    }
  }
}