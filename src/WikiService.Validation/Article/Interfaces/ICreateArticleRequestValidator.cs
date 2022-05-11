using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;

namespace LT.DigitalOffice.WikiService.Validation.Article.Interfaces
{
  [AutoInject]
  public interface ICreateArticleRequestValidator : IValidator<CreateArticleRequest>
  {
  }
}
