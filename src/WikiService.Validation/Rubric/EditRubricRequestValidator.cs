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
  public class EditRubricRequestValidator : ExtendedEditRequestValidator<DbRubric, EditRubricRequest>, IEditRubricRequestValidator
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

      AddСorrectOperations(nameof(EditRubricRequest.Name), new () { OperationType.Replace });
      AddСorrectOperations(nameof(EditRubricRequest.ParentId), new () { OperationType.Replace });
      AddСorrectOperations(nameof(EditRubricRequest.IsActive), new () { OperationType.Replace });

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
           x => bool.TryParse(x.value?.ToString(), out bool _), "Incorrect rubric IsActive format"
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

      When(x => x.Item2.Operations.Any(o =>
        (o.path.EndsWith(nameof(EditRubricRequest.ParentId), StringComparison.OrdinalIgnoreCase))
        || (o.path.EndsWith(nameof(EditRubricRequest.Name), StringComparison.OrdinalIgnoreCase))),
        () =>
        {
          RuleFor(x => x)
           .MustAsync(async (x, _) =>
           {
             Guid? _currentParentId = x.Item1.ParentId;
             string _currentRubricName = x.Item1.Name;

             foreach (Operation<EditRubricRequest> item in x.Item2.Operations)
             {
               if (item.path.EndsWith(nameof(EditRubricRequest.ParentId), StringComparison.OrdinalIgnoreCase))
               {
                 if (Guid.TryParse(item.value?.ToString(), out Guid parentId) == true || item.value is null)
                 {
                   _currentParentId = item.value is null
                    ? null
                    : parentId;
                 }
                 else
                 {
                   return false;
                 }
               }

               if (item.path.EndsWith(nameof(EditRubricRequest.Name), StringComparison.OrdinalIgnoreCase))
               {
                 _currentRubricName = item.value?.ToString();
               }
             }

             return (_currentParentId == x.Item1.ParentId && _currentRubricName == x.Item1.Name)
              ? true
              : !await _rubricRepository.DoesRubricNameExistAsync(_currentParentId, _currentRubricName);
           })
           .WithMessage("That name already exists.");
        });
    }  
  }
}
