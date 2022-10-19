using LT.DigitalOffice.WikiService.Validation.Article;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System;

namespace LT.DigitalOffice.WikiService.Business.Commands.Article.Edit
{
  public record EditSpecificArticleRequest : IRequest<bool>
  {
    public Guid Id { get; set; }
    public JsonPatchDocument<EditArticleRequest> Request { get; set; }
  }
}
