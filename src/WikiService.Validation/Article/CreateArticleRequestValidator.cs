using FluentValidation;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;
using LT.DigitalOffice.WikiService.Validation.Article.Interfaces;

namespace LT.DigitalOffice.WikiService.Validation.Article
{
  public class CreateArticleRequestValidator : AbstractValidator<CreateArticleRequest>, ICreateArticleRequestValidator
  {
    public CreateArticleRequestValidator()
    {
      RuleFor(article => article.Name)
        .NotEmpty().WithMessage("Name must not be empty.");
      RuleFor(article => article.Content)
        .NotEmpty().WithMessage("Content must not be empty.");

    }
  }
}
