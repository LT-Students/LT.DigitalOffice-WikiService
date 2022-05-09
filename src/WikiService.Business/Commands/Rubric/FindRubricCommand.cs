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
    private readonly IFindRubricMapper _findRubricMapper;
    private readonly IResponseCreator _responseCreator;

    public FindRubricCommand(
      IBaseFindFilterValidator baseFindValidator,
      IRubricRepository rubricRepository,
      IFindRubricMapper findRubricMapper,
      IResponseCreator responseCreator)
    {
      _baseFindValidator = baseFindValidator;
      _rubricRepository = rubricRepository;
      _findRubricMapper = findRubricMapper;
      _responseCreator = responseCreator;
    }

    public async Task<FindResultResponse<FindRubricRequest>> ExecuteAsync(FindRubricFilter filter)
    {
      if (!_baseFindValidator.ValidateCustom(filter, out List<string> errors))
      {
        return _responseCreator.CreateFailureFindResponse<FindRubricRequest>(HttpStatusCode.BadRequest, errors);
      }

      FindResultResponse<FindRubricRequest> response = new();

      (List<DbRubric> dbRubrics, int totalCount) = await _rubricRepository.FindAsync(filter);

      response.Body = dbRubrics?.Select(dbRubric => _findRubricMapper.Map(dbRubric)).ToList();
      response.TotalCount = totalCount;

      return response;
    }
  }
}
