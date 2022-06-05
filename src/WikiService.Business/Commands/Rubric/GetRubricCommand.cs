using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.WikiService.Business.Commands.Rubric.Interfaces;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric.Filters;
using LT.DigitalOffice.WikiService.Models.Dto.Responses.Rubric;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric
{
  public class GetRubricCommand : IGetRubricCommand
  {
    private readonly IRubricRepository _rubricRepository;
    private readonly IRubricResponseMapper _rubricResponseMapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreator _responseCreator;

    public GetRubricCommand(
      IRubricRepository rubricRepository,
      IRubricResponseMapper rubricResponseMapper,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreator responseCreator)
    {
      _rubricRepository = rubricRepository;
      _rubricResponseMapper = rubricResponseMapper;
      _httpContextAccessor = httpContextAccessor;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<RubricResponse>> ExecuteAsync(GetRubricFilter filter)
    {
      DbRubric dbRubric = await _rubricRepository.GetAsync(filter);
      
      OperationResultResponse<RubricResponse> response = new();

      response.Body = _rubricResponseMapper.Map(dbRubric);
     
      return response.Body == null
        ? _responseCreator.CreateFailureResponse<RubricResponse>(HttpStatusCode.NotFound)
:       response;
    }
  }
}
