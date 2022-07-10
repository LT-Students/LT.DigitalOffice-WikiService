using FluentValidation;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.File;
using LT.DigitalOffice.WikiService.Validation.File.Interfaces;

namespace LT.DigitalOffice.WikiService.Validation.File
{
  public class RemoveFilesRequestValidator : AbstractValidator<RemoveFilesRequest>, IRemoveFilesRequestValidator
  {
    public RemoveFilesRequestValidator()
    {
      RuleFor(request => request.FilesIds)
        .NotNull().WithMessage("List of files ids must not be null.")
        .NotEmpty().WithMessage("List of files ids must not be empty.")
        .ForEach(x =>
          x.NotEmpty().WithMessage("Files Id must not be empty."));
    }
  }
}