using FluentValidation;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.WikiService.Business.Commands.Rubric.Interfaces;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric
{
  public class EditRubricRequestValidator : ExtendedEditRequestValidator<DbRubric, EditRubricRequest>, IEditRubricRequestValidator
  {
    private readonly IDataProvider _provider;

    private async Task<bool> DoesExistAsync(Guid rubricId)
    {
      return await _provider.Rubrics.AsNoTracking().AnyAsync(x => x.Id == rubricId);
    }

    private async Task<bool> DoesRubricIsActiveAsync(Guid rubricId)
    {
      return await _provider.Rubrics.AsNoTracking().AnyAsync(x => x.Id == rubricId && x.IsActive);
    }

    private async Task<int> CountChildrenAsync(Guid? parentId)
    {
      return await _provider.Rubrics.AsNoTracking().CountAsync(x => x.ParentId == parentId);
    }

    private async Task HandleInternalPropertyValidationAsync(
      Operation<EditRubricRequest> requestedOperation,
      ValidationContext<(DbRubric, JsonPatchDocument<EditRubricRequest>)> context)
    {
      Context = context;
      RequestedOperation = requestedOperation;

      #region paths

      AddСorrectPaths(
        new List<string>
        {
          nameof(EditRubricRequest.Name),
          nameof(EditRubricRequest.ParentId),
          nameof(EditRubricRequest.IsActive),
          nameof(EditRubricRequest.Position)
        });

      AddСorrectOperations(nameof(EditRubricRequest.Name), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditRubricRequest.ParentId), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditRubricRequest.IsActive), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditRubricRequest.Position), new() { OperationType.Replace });

      #endregion

      #region Name

      AddFailureForPropertyIf(
        nameof(EditRubricRequest.Name),
        x => x == OperationType.Replace,
        new()
        {
          {
            x => !string.IsNullOrWhiteSpace(x.value?.ToString()),
            "Name must not be empty."
          },
          {
            x => x.value.ToString().Trim().Length < 101,
            "Name is too long."
          }
        }, CascadeMode.Stop);

      #endregion

      #region IsActive

      AddFailureForPropertyIf(
       nameof(EditRubricRequest.IsActive),
       x => x == OperationType.Replace,
       new()
       {
         {
           x => bool.TryParse(x.value?.ToString(), out bool _),
           "Incorrect rubric is active format"
         },
       });

      #endregion

      #region ParentId

      await AddFailureForPropertyIfAsync(
        nameof(EditRubricRequest.ParentId),
        x => x == OperationType.Replace,
        new()
        {
          {
            async (x) =>
            {
              if (x.value?.ToString() is null)
              {
                return true;
              }

              return Guid.TryParse(x.value.ToString(), out Guid parentId)
                ? await DoesExistAsync(parentId)
                : false;
            },
            "Parent id doesn`t exist."
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

    public EditRubricRequestValidator(
      IDataProvider provider)
    {
      _provider = provider;

      RuleForEach(x => x.Item2.Operations)
        .CustomAsync(async (x, context, _) => await HandleInternalPropertyValidationAsync(x, context));

      When(x => x.Item2.Operations.Any(o =>
        o.path.EndsWith(nameof(EditRubricRequest.ParentId), StringComparison.OrdinalIgnoreCase)
          || o.path.EndsWith(nameof(EditRubricRequest.IsActive), StringComparison.OrdinalIgnoreCase)),
        () =>
        {
          RuleFor(x => x)
            .MustAsync(async (x, _) =>
            {
              Guid? _currentParentId = x.Item1.ParentId;
              bool _currentIsActive = x.Item1.IsActive;

              foreach (Operation<EditRubricRequest> item in x.Item2.Operations)
              {
                if (item.path.EndsWith(nameof(EditRubricRequest.ParentId), StringComparison.OrdinalIgnoreCase)
                  && (Guid.TryParse(item.value.ToString(), out Guid parentId) || item.value is null))
                {
                  _currentParentId = item.value is null
                    ? null
                    : parentId;
                }
                else if (item.path.EndsWith(nameof(EditRubricRequest.IsActive), StringComparison.OrdinalIgnoreCase)
                  && bool.TryParse(item.value?.ToString(), out bool isActive))
                {
                  _currentIsActive = isActive;
                }
              }

              if ((_currentParentId != x.Item1.ParentId || _currentIsActive != x.Item1.IsActive)
                && _currentIsActive
                && _currentParentId.HasValue
                && !await DoesRubricIsActiveAsync(_currentParentId.Value))
              {
                return false;
              }

              return true;
            }).WithMessage("Active sub-rubric can't be in archive rubric.");
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
              Guid parentId = Guid.Empty;

              foreach (Operation<EditRubricRequest> item in x.Item2.Operations)
              {
                if (item.path.EndsWith(nameof(EditRubricRequest.ParentId), StringComparison.OrdinalIgnoreCase))
                {
                  Guid.TryParse(item.value.ToString(), out parentId);
                }
                else if (item.path.EndsWith(nameof(EditRubricRequest.Position), StringComparison.OrdinalIgnoreCase))
                {
                  int.TryParse(item.value?.ToString(), out position);
                }
              }

              if (parentId != Guid.Empty && position == 0
                || parentId != Guid.Empty && position > await CountChildrenAsync(parentId) + 1
                || parentId == Guid.Empty && position > await CountChildrenAsync(x.Item1.ParentId) + 1)
              {
                return false;
              }

              return true;
            }).WithMessage("Position is too big.");
        });
    }
  }
}
