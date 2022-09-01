using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Models.Dto.Responses.Wiki;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Wiki.Interfaces
{
  [AutoInject]
  public interface IGetWikiCommand
  {
    Task<OperationResultResponse<WikiResponse>> ExecuteAsync();
  }
}
