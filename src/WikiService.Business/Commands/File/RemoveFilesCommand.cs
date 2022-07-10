using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Broker.Publishes.Interfaces;
using LT.DigitalOffice.WikiService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.File;
using LT.DigitalOffice.WikiService.Validation.File.Interfaces;
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
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRemoveFilesRequestValidator _validator;
    private readonly IResponseCreator _responseCreator;
    private readonly IPublish _publish;

    public RemoveFilesCommand(
     IArticleFileRepository repository,
     IAccessValidator accessValidator,
     IRemoveFilesRequestValidator validator,
     IHttpContextAccessor httpContextAccessor,
     IResponseCreator responseCreator,
     IPublish publish)
    {
      _repository = repository;
      _accessValidator = accessValidator;
      _validator = validator;
      _httpContextAccessor = httpContextAccessor;
      _responseCreator = responseCreator;
      _publish = publish;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(RemoveFilesRequest request)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveWiki))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      if (!_validator.ValidateCustom(request, out List<string> errors))
      {
        return _responseCreator.CreateFailureResponse<bool>(
          HttpStatusCode.BadRequest,
          errors);
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
          errors);
      }

      return response;
    }
  }
}
