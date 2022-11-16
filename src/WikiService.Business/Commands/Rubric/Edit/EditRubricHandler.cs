using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.WikiService.Business.Commands.Rubric.Interfaces;
using LT.DigitalOffice.WikiService.Business.Commands.Wiki;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric.Edit
{
  public class EditRubricHandler : IRequestHandler<EditSpecificRubricRequest, bool>
  {
    private readonly IEditRubricRequestValidator _validator;
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMemoryCache _cache;

    private Task<ValueTuple<int, Guid>[]> GetPositionsAsync(Guid? parentId)
    {
      return _provider.Rubrics
        .Where(x => x.ParentId == parentId)
        .Select(x => new ValueTuple<int, Guid>(x.Position, x.Id))
        .ToArrayAsync();
    }

    private async Task<bool> EditPositionsAsync(
      ValueTuple<int, Guid>[] positions,
      Guid? parentId)
    {
      List<DbRubric> rubrics = await _provider.Rubrics.Where(x => x.ParentId == parentId).ToListAsync();

      foreach (DbRubric rubric in rubrics)
      {
        int position = positions.FirstOrDefault(x => x.Item2 == rubric.Id).Item1;
        rubric.Position = position == 0
          ? rubric.Position
          : position;
      }

      return true;
    }

    private async Task<bool> ChangePositionAsync(Guid id, int position, Guid? parentId)
    {
      ValueTuple<int, Guid>[] positions = await GetPositionsAsync(parentId);

      Array.Sort(positions);

      for (int i = 0; i < positions.Length; i++)
      {
        if (positions[i].Item2 == id)
        {
          if (positions[i].Item1 < position)
          {
            int j = i + 1;
            while (j < positions.Length && positions[j].Item1 <= position)
            {
              positions[j].Item1--;
              j++;
            }
          }
          else
          {
            int j = i - 1;
            while (j >= 0 && positions[j].Item1 >= position)
            {
              positions[j].Item1++;
              j--;
            }
          }

          positions[i].Item1 = position;
          break;
        }
      }

      return await EditPositionsAsync(positions, parentId);
    }

    private async Task<bool> ChangeRubricAsync(Guid id, int position, Guid? parentId, Guid? oldParentId)
    {
      ValueTuple<int, Guid>[] positions = await GetPositionsAsync(parentId);
      ValueTuple<int, Guid>[] oldPositions = await GetPositionsAsync(oldParentId);

      Array.Sort(positions);
      Array.Sort(oldPositions);

      for (int i = 0; i < oldPositions.Length; i++)
      {
        if (oldPositions[i].Item2 == id)
        {
          for (int j = i; j < oldPositions.Length - 1; j++)
          {
            oldPositions[j].Item1 = oldPositions[j + 1].Item1 - 1;
            oldPositions[j].Item2 = oldPositions[j + 1].Item2;
          }

          Array.Clear(oldPositions, oldPositions.Length - 1, 1);
          break;
        }
      }

      for (int i = 0; i < positions.Length; i++)
      {
        if (positions[i].Item1 == position)
        {
          for (int j = i; j < positions.Length; j++)
          {
            oldPositions[j].Item1 = oldPositions[j].Item1 + 1;
          }

          break;
        }
      }

      return await EditPositionsAsync(positions, parentId) && await EditPositionsAsync(oldPositions, oldParentId);
    }

    private Task<DbRubric> GetAsync(Guid rubricId, CancellationToken ct)
    {
      return _provider.Rubrics.FirstOrDefaultAsync(x => x.Id == rubricId, ct);
    }

    private async Task ChangeActivityAsync(Guid rubricId, bool isActivate)
    {
      List<DbRubric> rubrics = await _provider.Rubrics.Where(rubric => rubric.ParentId == rubricId).ToListAsync();

      foreach (DbRubric rubric in rubrics)
      {
        List<DbArticle> articles = await _provider.Articles.Where(article => article.RubricId == rubric.Id).ToListAsync();
        foreach (DbArticle article in articles)
        {
          article.IsActive = isActivate;
        }

        rubric.IsActive = isActivate;

        await ChangeActivityAsync(rubric.Id, isActivate);
      }
    }

    private async Task<bool> EditAsync(DbRubric dbRubric, JsonPatchDocument<DbRubric> request)
    {
      if (dbRubric is null || request is null)
      {
        return false;
      }

      Guid? parentId = dbRubric.ParentId;
      int position = dbRubric.Position;
      bool isAtive = dbRubric.IsActive;

      request.ApplyTo(dbRubric);

      if (parentId != dbRubric.ParentId)
      {
        await ChangeRubricAsync(
          id: dbRubric.Id,
          position: dbRubric.Position,
          parentId: dbRubric.ParentId,
          oldParentId: parentId);
      }
      else if (position != dbRubric.Position)
      {
        await ChangePositionAsync(
          id: dbRubric.Id,
          position: dbRubric.Position,
          parentId: dbRubric.ParentId);
      }

      dbRubric.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      dbRubric.ModifiedAtUtc = DateTime.UtcNow;

      if (dbRubric.IsActive != isAtive)
      {
        List<DbArticle> articles = await _provider.Articles.Where(article => article.RubricId == dbRubric.Id).ToListAsync();
        foreach (DbArticle article in articles)
        {
          article.IsActive = dbRubric.IsActive;
        }

        await ChangeActivityAsync(dbRubric.Id, dbRubric.IsActive);
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
      IHttpContextAccessor httpContextAccessor,
      IMemoryCache cache)
    {
      _validator = validator;
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
      _cache = cache;
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

      _cache.Remove(CacheKeys.WikiTreeWithDeactivated);
      _cache.Remove(CacheKeys.WikiTreeWithoutDeactivated);

      return await EditAsync(rubric, Map(request.Request));
    }
  }
}
