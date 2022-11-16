using FluentValidation;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.WikiService.Business.Commands.Article.Edit.Interfaces;
using LT.DigitalOffice.WikiService.Business.Commands.Rubric;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Article.Edit
{
  public class EditArticleRequestValidator : ExtendedEditRequestValidator<DbArticle, EditArticleRequest>, IEditArticleRequestValidator
  {
    private readonly IDataProvider _provider;

    private async Task<bool> DoesExistAsync(Guid rubricId)
    {
      return await _provider.Rubrics.AnyAsync(x => x.Id == rubricId);
    }

    private async Task<bool> DoesRubricIsActiveAsync(Guid rubricId)
    {
      return await _provider.Rubrics.AnyAsync(x => x.Id == rubricId && x.IsActive);
    }

    private async Task<int> CountChildrenAsync(Guid? parentId)
    {
      return await _provider.Rubrics.CountAsync(x => x.ParentId == parentId);
    }

    private async Task HandleInternalPropertyValidationAsync(
      Operation<EditArticleRequest> requestedOperation,
      ValidationContext<(DbArticle, JsonPatchDocument<EditArticleRequest>)> context)
    {
      RequestedOperation = requestedOperation;
      Context = context;

      #region Paths
      AddСorrectPaths(
        new List<string>
        {
          nameof(EditArticleRequest.Name),
          nameof(EditArticleRequest.Content),
          nameof(EditArticleRequest.IsActive),
          nameof(EditArticleRequest.RubricId),
          nameof(EditRubricRequest.Position)
        });

      AddСorrectOperations(nameof(EditArticleRequest.Name), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditArticleRequest.Content), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditArticleRequest.IsActive), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditArticleRequest.RubricId), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditArticleRequest.Position), new() { OperationType.Replace });
      #endregion

      #region Name
      AddFailureForPropertyIf(
        nameof(EditArticleRequest.Name),
        x => x == OperationType.Replace,
        new()
        {
          { x => !string.IsNullOrWhiteSpace(x.value?.ToString()), "Name can't be empty." },
          { x => x.value?.ToString().Trim().Length < 211, "Max lenght of article name is 210 symbols." },
        },
        CascadeMode.Stop);

      #endregion

      #region Content
      AddFailureForPropertyIf(
        nameof(EditArticleRequest.Content),
        x => x == OperationType.Replace,
        new()
        {
          { x => !string.IsNullOrWhiteSpace(x.value?.ToString()), "Article content can't be empty." }
        });
      #endregion

      #region IsActive
      AddFailureForPropertyIf(
        nameof(EditArticleRequest.IsActive),
        x => x == OperationType.Replace,
        new()
        {
          { x => bool.TryParse(x.value?.ToString(), out bool _), "Incorrect format of IsActive." }
        });
      #endregion

      #region RubricId
      await AddFailureForPropertyIfAsync(
        nameof(EditArticleRequest.RubricId),
        x => x == OperationType.Replace,
        new()
        {
          {
            async x => Guid.TryParse(x.value?.ToString(), out Guid _rubricId)
              ? await DoesExistAsync(_rubricId)
              : false,
            "This rubric doesn't exist."
          }
        });
      #endregion

      #region Position
      AddFailureForPropertyIf(
       nameof(EditRubricRequest.Position),
       x => x == OperationType.Replace,
       new()
       {
         {
           x => int.Parse(x.value?.ToString()) > 0,
           "Position must be greater than 0."
         },
       });
      #endregion
    }

    public EditArticleRequestValidator(
      IDataProvider provider)
    {
      _provider = provider;

      RuleForEach(x => x.Item2.Operations)
        .CustomAsync(async (x, context, _) => await HandleInternalPropertyValidationAsync(x, context));

      When(x => x.Item2.Operations.Any(o =>
        (o.path.EndsWith(nameof(EditRubricRequest.ParentId), StringComparison.OrdinalIgnoreCase))
          || (o.path.EndsWith(nameof(EditRubricRequest.IsActive), StringComparison.OrdinalIgnoreCase))),
        () =>
        {
          RuleFor(x => x)
            .MustAsync(async (x, _) =>
            {
              Guid _currentRubricId = x.Item1.RubricId;
              bool _currentIsActive = x.Item1.IsActive;

              foreach (Operation<EditArticleRequest> item in x.Item2.Operations)
              {
                if (item.path.EndsWith(nameof(EditRubricRequest.ParentId), StringComparison.OrdinalIgnoreCase)
                  && Guid.TryParse(item.value.ToString(), out Guid rybricId))
                {
                  _currentRubricId = rybricId;
                }
                else if (item.path.EndsWith(nameof(EditRubricRequest.IsActive), StringComparison.OrdinalIgnoreCase)
                  && bool.TryParse(item.value?.ToString(), out bool isActive))
                {
                  _currentIsActive = isActive;
                }
              }

              if ((_currentRubricId != x.Item1.RubricId || _currentIsActive != x.Item1.IsActive)
                && _currentIsActive
                && !await DoesRubricIsActiveAsync(_currentRubricId))
              {
                return false;
              }

              return true;
            }).WithMessage("Active article can't be in archive rubric.");
        });

      When(x => x.Item2.Operations.Any(o =>
        o.path.EndsWith(nameof(EditRubricRequest.ParentId), StringComparison.OrdinalIgnoreCase)
          || o.path.EndsWith(nameof(EditRubricRequest.Position), StringComparison.OrdinalIgnoreCase)),
        () =>
        {
          RuleFor(x => x)
            .MustAsync(async (x, _) =>
            {
              int position = 0;
              Guid rubricId = Guid.Empty;

              foreach (Operation<EditArticleRequest> item in x.Item2.Operations)
              {
                if (item.path.EndsWith(nameof(EditRubricRequest.ParentId), StringComparison.OrdinalIgnoreCase))
                {
                  Guid.TryParse(item.value.ToString(), out rubricId);
                }
                else if (item.path.EndsWith(nameof(EditRubricRequest.Position), StringComparison.OrdinalIgnoreCase))
                {
                  int.TryParse(item.value?.ToString(), out position);
                }
              }

              if (rubricId != Guid.Empty && position == 0
                || rubricId != Guid.Empty && position > await CountChildrenAsync(rubricId)
                || rubricId == Guid.Empty && position > await CountChildrenAsync(x.Item1.RubricId))
              {
                return false;
              }

              return true;
            }).WithMessage("Position is too big.");
        });
    }
  }
}