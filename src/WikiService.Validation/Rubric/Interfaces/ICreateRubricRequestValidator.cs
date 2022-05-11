using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Dto.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Validation.Rubric.Interfaces
{
  [AutoInject]
  public interface ICreateRubricRequestValidator : IValidator<CreateRubricRequest>
  {

  }
}
