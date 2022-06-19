﻿using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Business.Commands.Rubric.Interfaces;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric.Filters;
using LT.DigitalOffice.WikiService.Models.Dto.Responses.Rubric;
using System.Net;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric
{
  public class GetRubricCommand : IGetRubricCommand
  {
    private readonly IRubricRepository _rubricRepository;
    private readonly IRubricResponseMapper _rubricResponseMapper;
    private readonly IResponseCreator _responseCreator;

    public GetRubricCommand(
      IRubricRepository rubricRepository,
      IRubricResponseMapper rubricResponseMapper,
      IResponseCreator responseCreator)
    {
      _rubricRepository = rubricRepository;
      _rubricResponseMapper = rubricResponseMapper;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<RubricResponse>> ExecuteAsync(GetRubricFilter filter)
    {
      OperationResultResponse<RubricResponse> response = new();

      (DbRubric dbRubric, filter.IncludeSubRubrics) = await _rubricRepository.GetAsync(filter);

      response.Body = 
        _rubricResponseMapper.Map(dbRubric, filter.IncludeSubRubrics);

      return response.Body is null
        ? _responseCreator.CreateFailureResponse<RubricResponse>(HttpStatusCode.NotFound)
        : response;
    }
  }
}
