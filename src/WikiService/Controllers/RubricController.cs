using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Models.Dto.Requests;
using LT.DigitalOffice.WikiService.Models.Dto.Models;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric.Filters;
using LT.DigitalOffice.WikiService.Business.Commands.Rubric.Interfaces; 
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.JsonPatch;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric;

namespace LT.DigitalOffice.WikiService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class RubricController : ControllerBase
  {
    [HttpPost("create")]
    public async Task<OperationResultResponse<Guid?>> CreateAsync(
      [FromServices] ICreateRubricCommand command,
      [FromBody] CreateRubricRequest request)
    {
      return await command.ExecuteAsync(request);
    }

    [HttpGet("find")]
    public async Task<FindResultResponse<RubricInfo>> FindAsync(
      [FromServices] IFindRubricCommand command,
      [FromQuery] FindRubricFilter filter)
    {
      return await command.ExecuteAsync(filter);
    }

    [HttpPatch("edit")]
    public async Task<OperationResultResponse<bool>> EditAsync(
      [FromServices] IEditRubricCommand command,
      [FromQuery] Guid rubricId,
      [FromBody] JsonPatchDocument<EditRubricRequest> request)
    {
      return await command.ExecuteAsync(rubricId, request);
    }
  }
}