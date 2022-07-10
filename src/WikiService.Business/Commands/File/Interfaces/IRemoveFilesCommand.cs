using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.File;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.File.Interfaces
{
  [AutoInject]
  public interface IRemoveFilesCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(RemoveFilesRequest request);
  }
}
