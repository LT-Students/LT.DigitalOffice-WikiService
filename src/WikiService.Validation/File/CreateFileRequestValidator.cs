using FluentValidation;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Models.Dto.Models;
using LT.DigitalOffice.WikiService.Validation.File.Interfaces;

namespace LT.DigitalOffice.WikiService.Validation.File
{
  public class CreateFileRequestValidator : AbstractValidator<CreateFileRequest>, ICreateFileRequestValidator
  {
    public CreateFileRequestValidator(
      IArticleRepository articleRepository)
    {
      RuleFor(file => file.ArticleId)
      .MustAsync(async (articleId, _) => await articleRepository.DoesExistAsync(articleId))
      .WithMessage("This article id does not exist");
    }
  }
}