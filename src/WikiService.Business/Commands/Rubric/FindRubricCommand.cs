using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.WikiService.Business.Commands.Rubric.Interfaces;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Models.Dto.Requests;
using LT.DigitalOffice.WikiService.Mappers.Db.Interfaces;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.WikiService.Models.Db;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric
{
  public class FindRubricCommand : IFindRubricCommand
  {
    private readonly IBaseFindFilterValidator _baseFindValidator;
    private readonly IRubricRepository _rubricRepository;
    private readonly IRubricMapper _rubricMapper;
    private readonly IResponseCreator _responseCreator;

    public FindRubricCommand(
      IBaseFindFilterValidator baseFindValidator,
      IRubricRepository rubricRepository,
      IRubricMapper rubricMapper,
      IResponseCreator responseCreator)
    {
      _baseFindValidator = baseFindValidator;
      _rubricRepository = rubricRepository;
      _rubricMapper = rubricMapper;
      _responseCreator = responseCreator;
    }

    public async Task<FindResultResponse<Models.Dto.Requests.Rubric>> ExecuteAsync(FindRubricFilter filter)
    {
      if (!_baseFindValidator.ValidateCustom(filter, out List<string> errors))
      {
        return _responseCreator.CreateFailureFindResponse<Models.Dto.Requests.Rubric>(HttpStatusCode.BadRequest, errors);
      }

      FindResultResponse<Models.Dto.Requests.Rubric> response = new();

      (List<DbRubric> dbRubrics, int totalCount) = await _rubricRepository.FindAsync(filter);

      response.Body = dbRubrics?.Select(dbRubric => _rubricMapper.Map(dbRubric)).ToList();
      response.TotalCount = totalCount;

      return response;
    }
  }
}
