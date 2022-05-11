using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Models.Dto.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Interfaces
{
  [AutoInject]
  public interface ICreateRubricCommand
  {
    Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateRubricRequest request);
  }
}
