using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Business.Commands.Rubric.Interfaces;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Mappers.PatchDocument.Interfaces;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric;
using LT.DigitalOffice.WikiService.Validation.Rubric.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric
{
  public class EditRubricCommand : IEditRubricCommand
  {
    private readonly IEditRubricRequestValidator _validator;
    private readonly IAccessValidator _accessValidator;
    private readonly IPatchDbRubricMapper _mapper;
    private readonly IRubricRepository _rubricRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreator _responseCreator;

    public EditRubricCommand(
      IEditRubricRequestValidator validator,
      IAccessValidator accessValidator,
      IPatchDbRubricMapper mapper,
      IRubricRepository rubricRepository,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreator responseCreator)
    {
      _validator = validator;
      _accessValidator = accessValidator;
      _mapper = mapper;
      _rubricRepository = rubricRepository;
      _httpContextAccessor = httpContextAccessor;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid rubricId, JsonPatchDocument<EditRubricRequest> request)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveWiki))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      DbRubric rubric = await _rubricRepository.GetAsync(rubricId);

      if (rubric is null)
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.NotFound);
      }
      
      ValidationResult validationResult = await _validator.ValidateAsync((rubric, request));

      if (!validationResult.IsValid)
      {
        return _responseCreator.CreateFailureResponse<bool>
          (HttpStatusCode.BadRequest,
          validationResult.Errors.Select(e => e.ErrorMessage).ToList());
      }

      OperationResultResponse<bool> response = new();

      response.Body = await _rubricRepository.EditAsync(rubric, _mapper.Map(request));

      return response;
    }
  }
}
