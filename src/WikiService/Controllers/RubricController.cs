using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Business.Commands.Interfaces;
using LT.DigitalOffice.WikiService.Business.Commands.Rubric.Interfaces;
using LT.DigitalOffice.WikiService.Models.Dto.Models;
using LT.DigitalOffice.WikiService.Models.Dto.Requests;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric.Filters;
using LT.DigitalOffice.WikiService.Models.Dto.Responses.Rubric;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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

    [HttpGet("get")]
    public async Task<OperationResultResponse<RubricResponse>> GetAsync(
      [FromServices] IGetRubricCommand command,
      [FromQuery] GetRubricFilter filter)
    {
      return await command.ExecuteAsync(filter);
    }
  }
}