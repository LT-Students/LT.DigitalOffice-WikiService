using FluentValidation;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.WikiService.Business.Commands.Article.Edit.Interfaces;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    }

    public EditArticleRequestValidator(
      IDataProvider provider)
    {
      _provider = provider;

      RuleForEach(x => x.Item2.Operations)
        .CustomAsync(async (x, context, _) => await HandleInternalPropertyValidationAsync(x, context));
    }
  }
}