using FluentValidation;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;
using LT.DigitalOffice.WikiService.Validation.Article.Interfaces;

namespace LT.DigitalOffice.WikiService.Validation.Article
{
  public class CreateArticleRequestValidator : AbstractValidator<CreateArticleRequest>, ICreateArticleRequestValidator
  {
    public CreateArticleRequestValidator(
      IArticleRepository articleRepository)
    {

      CascadeMode = CascadeMode.Stop;

      RuleFor(article => article.RubricId)
        .NotEmpty().WithMessage("Rubric id must not be empty.")
        .MustAsync(async (rubricId, _) => await articleRepository.DoesExistAsync(rubricId))
          .WithMessage("This rubric id does not exist.");

      RuleFor(article => article.Name)
        .NotEmpty().WithMessage("Name must not be empty.");

      RuleFor(article => article)
        .MustAsync(async (article, _) => !await articleRepository.DoesSameArticleNameExistAsync(article.RubricId, article.Name))
        .WithMessage("This article name already exists.");

      RuleFor(article => article.Content)
        .NotEmpty().WithMessage("Content must not be empty.");



    }
  }
}
