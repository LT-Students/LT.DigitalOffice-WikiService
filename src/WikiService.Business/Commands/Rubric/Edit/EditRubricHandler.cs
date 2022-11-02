using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.WikiService.Business.Commands.Rubric.Interfaces;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric.Edit
{
  public class EditRubricHandler : IRequestHandler<EditSpecificRubricRequest, bool>
  {
    private readonly IEditRubricRequestValidator _validator;
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private Task<DbRubric> GetAsync(Guid rubricId, CancellationToken ct)
    {
      return _provider.Rubrics.FirstOrDefaultAsync(x => x.Id == rubricId, ct);
    }

    private async Task DisActivateAsync(Guid rubricId)
    {
      List<DbRubric> rubrics = await _provider.Rubrics.Where(rubric => rubric.ParentId == rubricId).ToListAsync();

      foreach (DbRubric rubric in rubrics)
      {
        List<DbArticle> articles = await _provider.Articles.Where(article => article.RubricId == rubric.Id).ToListAsync();
        foreach (DbArticle article in articles)
        {
          article.IsActive = false;
        }

        rubric.IsActive = false;

        await DisActivateAsync(rubric.Id);
      }
    }

    private async Task<bool> EditAsync(DbRubric dbRubric, JsonPatchDocument<DbRubric> request)
    {
      if (dbRubric is null || request is null)
      {
        return false;
      }

      bool isAtive = dbRubric.IsActive;
      request.ApplyTo(dbRubric);
      dbRubric.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      dbRubric.ModifiedAtUtc = DateTime.UtcNow;

      if (isAtive && dbRubric.IsActive != isAtive)
      {
        List<DbArticle> articles = await _provider.Articles.Where(article => article.RubricId == dbRubric.Id).ToListAsync();
        foreach (DbArticle article in articles)
        {
          article.IsActive = false;
        }

        await DisActivateAsync(dbRubric.Id);
      }

      await _provider.SaveAsync();

      return true;
    }

    private JsonPatchDocument<DbRubric> Map(JsonPatchDocument<EditRubricRequest> request)
    {
      if (request is null)
      {
        return null;
      }

      JsonPatchDocument<DbRubric> dbRequest = new JsonPatchDocument<DbRubric>();

      foreach (Operation<EditRubricRequest> item in request.Operations)
      {
        dbRequest.Operations.Add(new Operation<DbRubric>(item.op, item.path, item.from, item.value));
      }

      return dbRequest;
    }

    public EditRubricHandler(
      IEditRubricRequestValidator validator,
      IDataProvider provider,
      IHttpContextAccessor httpContextAccessor)
    {
      _validator = validator;
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(EditSpecificRubricRequest request, CancellationToken ct)
    {
      DbRubric rubric = await GetAsync(request.Id, ct);

      if (rubric is null)
      {
        return false;
      }

      ValidationResult validationResult = await _validator.ValidateAsync((rubric, request.Request), ct);
      if (!validationResult.IsValid)
      {
        throw new BadRequestException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
      }

      return await EditAsync(rubric, Map(request.Request));
    }
  }
}
