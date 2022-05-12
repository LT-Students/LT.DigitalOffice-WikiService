using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Models.Dto.Requests;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Filters;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric.Interfaces
{
  [AutoInject]
  public interface IFindRubricCommand
  {
    Task<FindResultResponse<Models.Dto.Requests.Rubric>> ExecuteAsync(FindRubricFilter filter);
  }
}
