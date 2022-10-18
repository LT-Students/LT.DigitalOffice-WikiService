using FluentValidation.Results;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.WikiService.Business.Commands.Article.Edit.Interfaces;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Article.Edit
{
  public class EditArticleHandler : IRequestHandler<EditSpecificArticleRequest, bool>
  {
    private readonly IDataProvider _provider;
    private readonly IEditArticleRequestValidator _validator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private JsonPatchDocument<DbArticle> Map(JsonPatchDocument<EditArticleRequest> request)
    {
      JsonPatchDocument<DbArticle> result = new JsonPatchDocument<DbArticle>();

      foreach (Operation<EditArticleRequest> item in request.Operations)
      {
        result.Operations.Add(new Operation<DbArticle>(item.op, item.path, item.from, item.value));
      }

      return result;
    }

    public EditArticleHandler(
       IEditArticleRequestValidator validator,
       IDataProvider provider,
       IHttpContextAccessor httpContextAccessor)
    {
      _validator = validator;
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(EditSpecificArticleRequest request, CancellationToken ct)
    {
      DbArticle dbArticle = await _provider.Articles.Include(a => a.Files)
        .FirstOrDefaultAsync(article => article.Id == request.Id);

      ValidationResult validationResult = await _validator.ValidateAsync((dbArticle, request.Request), ct);
      if (!validationResult.IsValid)
      {
        throw new BadRequestException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
      }

      JsonPatchDocument<DbArticle> article = Map(request.Request);

      article.ApplyTo(dbArticle);
      dbArticle.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      dbArticle.ModifiedAtUtc = DateTime.UtcNow;
      await _provider.SaveAsync();

      return true;
    }
  }
}