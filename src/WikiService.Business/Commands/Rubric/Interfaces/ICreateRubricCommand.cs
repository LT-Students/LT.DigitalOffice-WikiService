using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric.Interfaces
{
  [AutoInject]
  public interface ICreateRubricCommand
  {
    Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateRubricRequest request);
  }
}
