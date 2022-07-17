using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Broker.Publishes.Interfaces;
using LT.DigitalOffice.WikiService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.File;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.File
{
  public class RemoveFilesCommand : IRemoveFilesCommand
  {
    private readonly IArticleFileRepository _repository;
    private readonly IAccessValidator _accessValidator;
    private readonly IResponseCreator _responseCreator;
    private readonly IPublish _publish;

    public RemoveFilesCommand(
     IArticleFileRepository repository,
     IAccessValidator accessValidator,
     IHttpContextAccessor httpContextAccessor,
     IResponseCreator responseCreator,
     IPublish publish)
    {
      _repository = repository;
      _accessValidator = accessValidator;
      _responseCreator = responseCreator;
      _publish = publish;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(RemoveFilesRequest request)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveWiki))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      OperationResultResponse<bool> response = new();

      response.Body = await _repository.RemoveAsync(request.FilesIds);

      if (response.Body)
      {
        await _publish.RemoveFilesAsync(request.FilesIds);
      }

      else
      {
        return _responseCreator.CreateFailureResponse<bool>(
          HttpStatusCode.BadRequest,
          response.Errors);
      }

      return response;
    }
  }
}
