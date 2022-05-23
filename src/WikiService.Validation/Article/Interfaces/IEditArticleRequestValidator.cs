using FluentValidation;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;
using System;

namespace LT.DigitalOffice.WikiService.Validation.Article.Interfaces
{
  [AutoInject]
  public interface IEditArticleRequestValidator : IValidator<(Guid, JsonPatchDocument<EditArticleRequest>)>
  {
  }
}