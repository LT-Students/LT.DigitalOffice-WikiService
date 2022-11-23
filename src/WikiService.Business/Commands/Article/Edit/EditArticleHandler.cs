using Automatonymous.Activities;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.ProjectService.Broker.Publishes.Interfaces;
using LT.DigitalOffice.WikiService.Business.Commands.Article.Edit.Interfaces;
using LT.DigitalOffice.WikiService.Business.Commands.Wiki;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
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
    private readonly IMemoryCache _cache;
    private readonly IPublish _publish;

    private Task<ValueTuple<int, Guid>[]> GetPositionsAsync(Guid rubricId)
    {
      return _provider.Articles
        .Where(x => x.RubricId == rubricId)
        .Select(x => new ValueTuple<int, Guid>(x.Position, x.Id))
        .ToArrayAsync();
    }

    private async Task<bool> EditPositionsAsync(
      ValueTuple<int, Guid>[] positions,
      Guid rubricId)
    {
      List<DbArticle> articles = await _provider.Articles.Where(x => x.RubricId == rubricId).ToListAsync();

      foreach (DbArticle atricle in articles)
      {
        int position = positions.FirstOrDefault(x => x.Item2 == atricle.Id).Item1;
        atricle.Position = position == 0
          ? atricle.Position
          : position;
      }

      return true;
    }

    private async Task<bool> ChangePositionAsync(Guid id, int position, Guid rubricId)
    {
      ValueTuple<int, Guid>[] positions = await GetPositionsAsync(rubricId);

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

      return await EditPositionsAsync(positions, rubricId);
    }

    private async Task<bool> ChangeRubricAsync(Guid id, int position, Guid rubricId, Guid oldRubricId)
    {
      ValueTuple<int, Guid>[] positions = await GetPositionsAsync(rubricId);
      ValueTuple<int, Guid>[] oldPositions = await GetPositionsAsync(oldRubricId);

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

      return await EditPositionsAsync(positions, rubricId) && await EditPositionsAsync(oldPositions, oldRubricId);
    }

    private JsonPatchDocument<DbArticle> Map(JsonPatchDocument<EditArticleRequest> request)
    {
      JsonPatchDocument<DbArticle> result = new JsonPatchDocument<DbArticle>();

      foreach (Operation<EditArticleRequest> item in request.Operations)
      {
        result.Operations.Add(new Operation<DbArticle>(item.op, item.path, item.from, item.value));
      }

      return result;
    }

    private async Task<bool> EditAsync(DbArticle dbArticle, JsonPatchDocument<DbArticle> request)
    {
      Guid rubricId = dbArticle.RubricId;
      int position = dbArticle.Position;
      bool isActive = dbArticle.IsActive;
      request.ApplyTo(dbArticle);

      if (rubricId != dbArticle.RubricId)
      {
        await ChangeRubricAsync(
          id: dbArticle.Id,
          position: dbArticle.Position, 
          rubricId: dbArticle.RubricId,
          oldRubricId: rubricId);
      }
      else if (position != dbArticle.Position)
      {
        await ChangePositionAsync(
          id: dbArticle.Id,
          position: dbArticle.Position,
          rubricId: dbArticle.RubricId);
      }

      if (isActive && !dbArticle.IsActive)
      {
        await _publish.RemoveFilesAsync(dbArticle.Files.Select(file => file.FileId).ToList());
        dbArticle.Files.Clear();
      }

      dbArticle.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      dbArticle.ModifiedAtUtc = DateTime.UtcNow;
      await _provider.SaveAsync();

      return true;
    }

    public EditArticleHandler(
       IEditArticleRequestValidator validator,
       IDataProvider provider,
       IHttpContextAccessor httpContextAccessor,
       IMemoryCache cache,
       IPublish publish)
    {
      _validator = validator;
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
      _cache = cache;
      _publish = publish;
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

      _cache.Remove(CacheKeys.WikiTreeWithDeactivated);
      _cache.Remove(CacheKeys.WikiTreeWithoutDeactivated);

      return await EditAsync(dbArticle, Map(request.Request));
    }
  }
}