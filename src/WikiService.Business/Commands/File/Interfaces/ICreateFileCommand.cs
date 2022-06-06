using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Models.Dto.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.File.Interfaces
{
  [AutoInject]
  public interface ICreateFileCommand
  {
    Task<OperationResultResponse<List<Guid>>> ExecuteAsync(CreateFileRequest request);
  }
}
