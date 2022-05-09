using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Models.Dto.Requests;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.WikiService.Business.Commands.Rubric.Interfaces; 
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.WikiService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class RubricController : ControllerBase
  {
    [HttpGet("find")]
    public async Task<FindResultResponse<FindRubricRequest>> FindAsync(
      [FromServices] IFindRubricCommand command,
      [FromQuery] FindRubricFilter filter)
    {
      return await command.ExecuteAsync(filter);
    }
  }
}