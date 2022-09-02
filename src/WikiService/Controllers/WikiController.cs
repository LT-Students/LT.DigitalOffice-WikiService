using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Business.Commands.Wiki.Interfaces;
using LT.DigitalOffice.WikiService.Models.Dto.Responses.Wiki;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class WikiController
  {
    [HttpGet("get")]
    public async Task<OperationResultResponse<List<WikiResponse>>> GetAsync(
      [FromServices] IGetWikiCommand command)
    {
      return await command.ExecuteAsync();
    }
  }
}
