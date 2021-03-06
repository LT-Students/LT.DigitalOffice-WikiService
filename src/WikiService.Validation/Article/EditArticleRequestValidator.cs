using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;
using LT.DigitalOffice.WikiService.Validation.Article.Interfaces;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Validation.Article
{
  public class EditArticleRequestValidator : ExtendedEditRequestValidator<DbArticle, EditArticleRequest>, IEditArticleRequestValidator
  {
    private readonly IArticleRepository _articleRepository;
    private readonly IRubricRepository _rubricRepository;

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
          { x => !string.IsNullOrWhiteSpace(x.value?.ToString()), "Name can't be empty." },
          { x => x.value?.ToString().Trim().Length < 151, "Max lenght of article name is 150 symbols." },
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
          { x => bool.TryParse(x.value?.ToString(), out bool _), "Incorrect format of IsActive." },
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
              ? await _rubricRepository.DoesExistAsync(_rubricId)
              : false,
              "This rubric id doesn't exist."
          }
        });
      #endregion
    }

    public EditArticleRequestValidator(IArticleRepository articleRepository, IRubricRepository rubricRepository)
      {
        _articleRepository = articleRepository;
        _rubricRepository = rubricRepository;

        RuleForEach(x => x.Item2.Operations)
          .CustomAsync(async (x, context, _) => await HandleInternalPropertyValidationAsync(x, context));

        When(x => x.Item2.Operations.Any(o =>
          (o.path.EndsWith(nameof(EditArticleRequest.RubricId), StringComparison.OrdinalIgnoreCase))
          || (o.path.EndsWith(nameof(EditArticleRequest.Name), StringComparison.OrdinalIgnoreCase))),
          () =>
          {
            RuleFor(x => x)
             .MustAsync(async (x, _) =>
             {
               Guid _currentRubricId = x.Item1.RubricId;
               string _currentArticleName = x.Item1.Name;

               foreach (Operation<EditArticleRequest> item in x.Item2.Operations)
               {
                 _currentRubricId = item.path.EndsWith(nameof(EditArticleRequest.RubricId), StringComparison.OrdinalIgnoreCase)
                   ? Guid.TryParse(item.value?.ToString(), out Guid _rubricIdParseResult) ? _rubricIdParseResult : _currentRubricId
                   : _currentRubricId;

                 _currentArticleName = item.path.EndsWith(nameof(EditArticleRequest.Name), StringComparison.OrdinalIgnoreCase)
                  ? item.value?.ToString()
                  : _currentArticleName;
               }

               return (_currentRubricId == x.Item1.RubricId && _currentArticleName == x.Item1.Name)
                ? true
                : !await _articleRepository.DoesSameNameExistAsync(_currentRubricId, _currentArticleName);
             })
             .WithMessage("That article name already exists in this rubric.");
          });
      }
    }
  }