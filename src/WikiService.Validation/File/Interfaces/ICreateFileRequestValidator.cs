using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Dto.Models;

namespace LT.DigitalOffice.WikiService.Validation.File.Interfaces
{
  [AutoInject]
  public interface ICreateFileRequestValidator : IValidator<CreateFileRequest>
  {
  }
}
