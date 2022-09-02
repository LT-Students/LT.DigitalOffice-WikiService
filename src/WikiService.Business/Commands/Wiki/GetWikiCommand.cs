using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Business.Commands.Wiki.Interfaces;
using LT.DigitalOffice.WikiService.Models.Dto.Responses.Wiki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Wiki
{
  public class GetWikiCommand : IGetWikiCommand
  {
    public Task<OperationResultResponse<List<WikiResponse>>> ExecuteAsync()
    {
      throw new NotImplementedException();
    }
  }
}
