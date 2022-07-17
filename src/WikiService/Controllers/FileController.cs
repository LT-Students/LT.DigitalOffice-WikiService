using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.File;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class FileController : Controller
  {
    [HttpDelete("remove")]
    public async Task<OperationResultResponse<bool>> RemoveAsync(
     [FromServices] IRemoveFilesCommand command,
     [FromBody] RemoveFilesRequest request)
    {
      return await command.ExecuteAsync(request);
    }
  }
}
