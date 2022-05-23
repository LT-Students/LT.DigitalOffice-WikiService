using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.Kernel.Validators;
using Microsoft.AspNetCore.JsonPatch.Operations;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;
using LT.DigitalOffice.WikiService.Validation.Article.Interfaces;
using LT.DigitalOffice.WikiService.Data.Interfaces;

namespace LT.DigitalOffice.WikiService.Validation.Article
{
  public class EditArticleRequestValidator : ExtendedEditRequestValidator<Guid, EditArticleRequest>, IEditArticleRequestValidator
  {
    private readonly IArticleRepository _articleRepository;
    private readonly IRubricRepository _rubricRepository;
    //private Guid _rubricId;
    private async Task HandleInternalPropertyValidationAsync(Operation<EditArticleRequest> requestedOperation, CustomContext context)
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
          nameof(EditArticleRequest.RubricId)
        });

      AddСorrectOperations(nameof(EditArticleRequest.Name), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditArticleRequest.Content), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditArticleRequest.IsActive), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditArticleRequest.RubricId), new() { OperationType.Replace });
      #endregion

      #region Name
      AddFailureForPropertyIf(
        nameof(EditArticleRequest.Name),
        x => x == OperationType.Replace,
        new()
        {
          { x => !string.IsNullOrEmpty(x.value?.ToString()), "Name can't be empty." },
          { x => x.value.ToString().Length < 151, "Max lenght of article name is 150 symbols." },
        },
        CascadeMode.Stop);

      #endregion

      #region Content
      AddFailureForPropertyIf(
        nameof(EditArticleRequest.Content),
        x => x == OperationType.Replace,
        new()
        {
          { x => x.value == null, "Article content can't be empty." }
        });
      #endregion

      #region IsActive
      AddFailureForPropertyIf(
        nameof(EditArticleRequest.IsActive),
        x => x == OperationType.Replace,
        new()
        {
          { x => bool.TryParse(x.value?.ToString(), out bool _), "Incorrect format of IsActive." },
        });
      #endregion

      #region RubricId
      AddFailureForPropertyIf(
        nameof(EditArticleRequest.RubricId),
        x => x == OperationType.Replace,
        new()
        {
          { x => Guid.TryParse(x.value.ToString(), out Guid _), "Incorrect format of RubricId." },
        });

      await AddFailureForPropertyIfAsync(
        nameof(EditArticleRequest.RubricId),
        x => x == OperationType.Replace,
        new()
        {
          {
            async x => !await _rubricRepository.DoesExistAsync(
              Guid.TryParse(x.value.ToString(), out _rubricId) ? _rubricId : default
              ),
            "This rubric id doesn't exist."
          }
        });
      #endregion
    }
      public EditArticleRequestValidator (IArticleRepository articleRepository, IRubricRepository rubricRepository)
      {
        _articleRepository = articleRepository;
        _rubricRepository = rubricRepository;
        Guid _rubricId = default;

        RuleFor(x => x)
          .MustAsync(async (x, _) =>
          {
            foreach (Operation<EditArticleRequest> item in x.Item2.Operations)
            {
              _rubricId = item.path.EndsWith(nameof(EditArticleRequest.RubricId)) ? Guid.Parse(item.value.ToString()) : x.Item1;
            }
            foreach (Operation<EditArticleRequest> item in x.Item2.Operations)
            {
              return item.path.EndsWith(nameof(EditArticleRequest.Name))
                ? !await _articleRepository.DoesSameNameExistAsync(_rubricId, item.value.ToString())
                : true;
            }
          })
          .WithMessage("That article name already exists in this rubric.");

          RuleForEach(x => x.Item2.Operations)
            .CustomAsync(async (x, context, _) => await HandleInternalPropertyValidationAsync(x, context));
      }
  }
}