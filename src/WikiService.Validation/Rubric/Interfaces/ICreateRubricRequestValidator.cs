using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric;

namespace LT.DigitalOffice.WikiService.Validation.Rubric.Interfaces
{
  [AutoInject]
  public interface ICreateRubricRequestValidator : IValidator<CreateRubricRequest>
  {
  }
}
