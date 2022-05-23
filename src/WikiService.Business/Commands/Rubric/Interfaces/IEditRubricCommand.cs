using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric.Interfaces
{
  [AutoInject]
  public interface IEditRubricCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(Guid rubricId, JsonPatchDocument<EditRubricRequest> request);
  }
}
