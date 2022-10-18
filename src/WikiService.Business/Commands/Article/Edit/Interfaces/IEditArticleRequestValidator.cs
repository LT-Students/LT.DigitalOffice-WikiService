using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Db;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.WikiService.Business.Commands.Article.Edit.Interfaces
{
  [AutoInject]
  public interface IEditArticleRequestValidator : IValidator<(DbArticle, JsonPatchDocument<EditArticleRequest>)>
  {
  }
}
