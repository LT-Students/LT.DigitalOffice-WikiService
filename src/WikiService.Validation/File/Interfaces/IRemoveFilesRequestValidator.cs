using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.File;

namespace LT.DigitalOffice.WikiService.Validation.File.Interfaces
{
  [AutoInject]
  public interface IRemoveFilesRequestValidator : IValidator<RemoveFilesRequest>
  {
  }
}
