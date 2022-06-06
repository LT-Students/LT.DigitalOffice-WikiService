using FluentValidation.Results;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Models.File;
using LT.DigitalOffice.Models.Broker.Publishing.Subscriber.File;
using LT.DigitalOffice.WikiService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Mappers.Db.Interfaces;
using LT.DigitalOffice.WikiService.Mappers.Models.Interfaces;
using LT.DigitalOffice.WikiService.Models.Dto.Models;
using LT.DigitalOffice.WikiService.Validation.File.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.File
{
  public class CreateFileCommand : ICreateFileCommand
  {
    private readonly IDbArticleFileMapper _mapper;
    private readonly IArticleFileRepository _repository;
    private readonly ICreateFileRequestValidator _validator;
    private readonly IBus _bus;
    private readonly ILogger<CreateFileCommand> _logger;
    private readonly IAccessValidator _accessValidator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreator _responseCreator;
    private readonly IFileDataMapper _fileDataMapper;

    public CreateFileCommand(
      IDbArticleFileMapper mapper,
      IArticleFileRepository repository,
      ICreateFileRequestValidator validator,
      IBus bus,
      ILogger<CreateFileCommand> logger,
      IAccessValidator accessValidator,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreator responseCreator,
      IFileDataMapper fileDataMapper)
    {
      _mapper = mapper;
      _repository = repository;
      _validator = validator;
      _bus = bus;
      _logger = logger;
      _accessValidator = accessValidator;
      _httpContextAccessor = httpContextAccessor;
      _responseCreator = responseCreator;
      _fileDataMapper = fileDataMapper;
    }

    private async Task<bool> CreateFilesAsync(List<FileData> files, List<string> errors)
    {
      if (files == null || !files.Any())
      {
        return false;
      }

      await _bus.Publish<ICreateFilesPublish>(ICreateFilesPublish.CreateObj(
        files, _httpContextAccessor.HttpContext.GetUserId()));

      return true;
    }
    public async Task<OperationResultResponse<List<Guid>>> ExecuteAsync(CreateFileRequest request)
    {
      ValidationResult validationResult = await _validator.ValidateAsync(request);

      if (!validationResult.IsValid)
      {
        return _responseCreator.CreateFailureResponse<List<Guid>>(
          HttpStatusCode.BadRequest,
          validationResult.Errors.Select(vf => vf.ErrorMessage).ToList());
      }
      OperationResultResponse<List<Guid>> response = new();

      List<FileData> files = request.Files?.Select(x => _fileDataMapper.Map(x)).ToList();

      await CreateFilesAsync(files, response.Errors);

      if (response.Errors.Any())
      {
        return _responseCreator.CreateFailureResponse<List<Guid>>(HttpStatusCode.BadRequest, response.Errors);
      }

      response.Body = await _repository.CreateAsync(files.Select(x =>
        _mapper.Map(x.Id, request.ArticleId)).ToList());

      response.Status = OperationResultStatusType.FullSuccess;
      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

      return response;
    }
  }
}
