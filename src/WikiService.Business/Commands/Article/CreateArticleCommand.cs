using FluentValidation.Results;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Business.Commands.Article.Interfaces;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Mappers.Db.Interfaces;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;
using LT.DigitalOffice.WikiService.Validation.Article.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Article
{
  public class CreateArticleCommand : ICreateArticleCommand
  {
    private readonly IArticleRepository _repository;
    private readonly IDbArticleMapper _mapper;
    private readonly ICreateArticleRequestValidator _validator;
    private readonly IResponseCreator _responseCreator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAccessValidator _accessValidator;

    public CreateArticleCommand(
     IArticleRepository repository,
     IDbArticleMapper mapper,
     ICreateArticleRequestValidator validator,
     IResponseCreator responseCreator,
     IHttpContextAccessor httpContextAccessor,
     IAccessValidator accessValidator)
    {
      _repository = repository;
      _mapper = mapper;
      _validator = validator;
      _responseCreator = responseCreator;
      _httpContextAccessor = httpContextAccessor;
      _accessValidator = accessValidator;
    }

    public async Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateArticleRequest request)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveWiki))
      {
        return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.Forbidden);
      }

      ValidationResult validationResult = await _validator.ValidateAsync(request);

      if (!validationResult.IsValid)
      {
        return _responseCreator.CreateFailureResponse<Guid?>(
          HttpStatusCode.BadRequest,
          validationResult.Errors.Select(vf => vf.ErrorMessage).ToList());
      }

      OperationResultResponse<Guid?> response = new();
      response.Body = await _repository.CreateAsync(_mapper.Map(request));

      if (response.Body is null)
      {
        return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.BadRequest);
      }

      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

      return response;
    }
  }
}
