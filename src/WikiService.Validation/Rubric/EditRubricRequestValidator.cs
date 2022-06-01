using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric;
using LT.DigitalOffice.WikiService.Validation.Rubric.Interfaces;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Validation.Rubric
{
  public class EditRubricRequestValidator : ExtendedEditRequestValidator<Guid, EditRubricRequest>, IEditRubricRequestValidator
  {
    private readonly IRubricRepository _rubricRepository;

    private async Task HandleInternalPropertyValidationAsync(
      Operation<EditRubricRequest> requestedOperation,
      CustomContext context)
    {
      Context = context;
      RequestedOperation = requestedOperation;

      #region paths

      AddСorrectPaths(
        new List<string>
        {
          nameof(EditRubricRequest.Name),
          nameof(EditRubricRequest.ParentId),
          nameof(EditRubricRequest.IsActive)
        });

      AddСorrectOperations(nameof(EditRubricRequest.Name), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditRubricRequest.ParentId), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditRubricRequest.IsActive), new List<OperationType> { OperationType.Replace });

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
            x => x.value.ToString().Trim().Length <= 150,
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
           x => bool.TryParse(x.value?.ToString(), out bool _), "Incorrect rubric is active format"
         },
       }, CascadeMode.Stop);

      #endregion

      #region ParentId

      await AddFailureForPropertyIfAsync(
        nameof(EditRubricRequest.ParentId),
        x => x == OperationType.Replace,
        new()
        {
          {
            x => Task.FromResult(
              string.IsNullOrEmpty(x.value?.ToString())
              ? true
              : Guid.TryParse(x.value.ToString(), out Guid result)),
            "Incorrect format of ParentId."
          },
          {
            async (x) =>
            {
              if (x.value?.ToString() is null)
              {
                return true;
              }

              if (!Guid.TryParse(x.value?.ToString(), out Guid parentId))
              {
                return false;
              }

              return await _rubricRepository.DoesExistAsync(parentId);
            },
            "Parent id doesn`t exist."
          }
        }, CascadeMode.Stop);
    }

    #endregion

    public EditRubricRequestValidator(
        IRubricRepository rubricRepository)
    {
      _rubricRepository = rubricRepository;

      RuleForEach(x => x.Item2.Operations)
        .CustomAsync(async (x, context, _) => await HandleInternalPropertyValidationAsync(x, context));

      When(x => x.Item2.Operations.Any(o => o.path.EndsWith(nameof(EditRubricRequest.ParentId), StringComparison.OrdinalIgnoreCase)) == true
        && x.Item2.Operations.Any(o => o.path.EndsWith(nameof(EditRubricRequest.Name), StringComparison.OrdinalIgnoreCase)) == true,
        () =>
        {
          RuleFor(x => x)
            .MustAsync(async (x, _) =>
            {
              string opValue = x.Item2.Operations.FirstOrDefault(
                  o => o.path.EndsWith(nameof(EditRubricRequest.ParentId), StringComparison.OrdinalIgnoreCase)).value?.ToString();

              if (opValue is null)
              {
                return !await _rubricRepository.DoesRubricNameExistAsync(
                  null,
                  x.Item2.Operations.FirstOrDefault(
                    o => o.path.EndsWith(nameof(EditRubricRequest.Name), StringComparison.OrdinalIgnoreCase)).value?.ToString());
              }
              else if (Guid.TryParse(opValue, out Guid parentId))
              {
                return !await _rubricRepository.DoesRubricNameExistAsync(
                  parentId,
                  x.Item2.Operations.FirstOrDefault(
                    o => o.path.EndsWith(nameof(EditRubricRequest.Name), StringComparison.OrdinalIgnoreCase)).value?.ToString());
              }
              
              return false;
            })
            .WithMessage("Name already exists.");
        });

      When(x => !x.Item2.Operations.Any(o => o.path.EndsWith(nameof(EditRubricRequest.Name), StringComparison.OrdinalIgnoreCase))
        && x.Item2.Operations.Any(o => o.path.EndsWith(nameof(EditRubricRequest.ParentId), StringComparison.OrdinalIgnoreCase)),
        () =>
        {
          RuleFor(x => x)
            .MustAsync(async (x, _) =>
            {
              DbRubric oldRubric = await _rubricRepository.GetAsync(x.Item1);
              string rubricName = oldRubric.Name;
              string opValue = x.Item2.Operations.FirstOrDefault(
                  o => o.path.EndsWith(nameof(EditRubricRequest.ParentId), StringComparison.OrdinalIgnoreCase)).value?.ToString();

              if (opValue is null)
              {
                return !await _rubricRepository.DoesRubricNameExistAsync(null, rubricName);
              }
              else if (Guid.TryParse(opValue, out Guid parentId))
              {
                return !await _rubricRepository.DoesRubricNameExistAsync(parentId, rubricName);
              }
              
              return false;
            })
            .WithMessage("Name already exists.");
        });

      When(x => !x.Item2.Operations.Any(o => o.path.EndsWith(nameof(EditRubricRequest.ParentId), StringComparison.OrdinalIgnoreCase))
        && x.Item2.Operations.Any(o => o.path.EndsWith(nameof(EditRubricRequest.Name), StringComparison.OrdinalIgnoreCase)),
        () =>
        {
          RuleFor(x => x)
            .MustAsync(async (x, _) =>
            {
              DbRubric oldRubric = await _rubricRepository.GetAsync(x.Item1);
              Guid? rubricParentId = oldRubric.ParentId;
              
              return !await _rubricRepository.DoesRubricNameExistAsync(
                rubricParentId,
                x.Item2.Operations.FirstOrDefault(
                  o => o.path.EndsWith(nameof(EditRubricRequest.Name), StringComparison.OrdinalIgnoreCase)).value?.ToString());
            })
            .WithMessage("Name already exists.");
        });
    }
  }
}
