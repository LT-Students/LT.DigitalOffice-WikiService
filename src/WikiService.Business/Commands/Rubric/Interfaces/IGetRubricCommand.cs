using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric.Filters;
using LT.DigitalOffice.WikiService.Models.Dto.Responses.Rubric;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric.Interfaces
{
  [AutoInject]
  public interface IGetRubricCommand
  {
    Task<OperationResultResponse<RubricResponse>> ExecuteAsync(GetRubricFilter filter);
  }
}
