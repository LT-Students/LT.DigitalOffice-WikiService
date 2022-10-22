using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric.Create
{
  public class CreateRubricHandler : IRequestHandler<CreateRubricRequest, Guid?>
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDataProvider _provider;

    private async Task<Guid?> CreateAsync(DbRubric dbRubric)
    {
      if (dbRubric is null)
      {
        return null;
      }

      _provider.Rubrics.Add(dbRubric);
      await _provider.SaveAsync();

      return dbRubric.Id;
    }

    private DbRubric Map(CreateRubricRequest request)
    {
      if (request is null)
      {
        return null;
      }

      return new DbRubric
      {
        Id = Guid.NewGuid(),
        Name = request.Name.Trim(),
        ParentId = request.ParentId,
        IsActive = true,
        CreatedAtUtc = DateTime.UtcNow,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId()
      };
    }

    public CreateRubricHandler(
      IHttpContextAccessor httpContextAccessor,
      IDataProvider provider)
    {
      _httpContextAccessor = httpContextAccessor;
      _provider = provider;
    }

    public async Task<Guid?> Handle(CreateRubricRequest request, CancellationToken cancellationToken)
    {
      return await CreateAsync(Map(request));
    }
  }
}
