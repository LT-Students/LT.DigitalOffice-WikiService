using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric;
using LT.DigitalOffice.WikiService.Validation.Rubric.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Validation.Rubric
{
  public class EditRubricRequestValidator : ExtendedEditRequestValidator<Guid, EditRubricRequest>, IEditRubricRequestValidator
  {
    private readonly IRubricRepository _rubricRepository;

    private async Task HandleInternalPropertyValidationAsync(Guid rubricId, Operation<EditRubricRequest> requestedOperation, CustomContext context)
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
        new Dictionary<Func<Operation<EditRubricRequest>, bool>, string>
        {
          { x => !string.IsNullOrEmpty(x.value?.ToString().Trim()), "Name must not be empty." },
          { x => x.value.ToString().Trim().Length <= 150, "Name is too long." },
        }, CascadeMode.Stop);

      /*await AddFailureForPropertyIfAsync(
        nameof(EditRubricRequest.Name),
        x => x == OperationType.Replace,
        new Dictionary<Func<Operation<EditRubricRequest>, Task <bool>>, string>
        {
          {
            async (x) =>
            {
              if (!Guid.TryParse(x.value?.ToString(), out Guid parentId))
              {
                return false;
              }

              !await _rubricRepository.DoesRubricNameExistAsync(rubricId, name), "The rubric name already exist.") }
        }, CascadeMode.Stop);*/


      #endregion

      #region IsActive

      AddFailureForPropertyIf(
       nameof(EditRubricRequest.IsActive),
       x => x == OperationType.Replace,
       new Dictionary<Func<Operation<EditRubricRequest>, bool>, string>
       {
         {
           x => bool.TryParse(x.value?.ToString(), out bool _), "Incorrect rubric is active format"
         },
       }, CascadeMode.Stop);

      #endregion

      #region ParentId

      AddFailureForPropertyIf(
        nameof(EditRubricRequest.ParentId),
        x => x == OperationType.Replace,
        new Dictionary<Func<Operation<EditRubricRequest>, bool>, string>
        {
          {
            x => string.IsNullOrEmpty(
              x.value?.ToString())? true :
            Guid.TryParse(x.value.ToString(), out Guid result),
            "Incorrect format of ParentId."
          }
        }, CascadeMode.Stop);

      await AddFailureForPropertyIfAsync(
        nameof(EditRubricRequest.ParentId),
        x => x == OperationType.Replace,
        new Dictionary<Func<Operation<EditRubricRequest>, Task<bool>>, string>
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
        .CustomAsync(async (x, context, _) => await HandleInternalPropertyValidationAsync(rubricId, x, context));
    }
  }

}
