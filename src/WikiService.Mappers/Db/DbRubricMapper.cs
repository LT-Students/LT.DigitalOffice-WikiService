using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.WikiService.Mappers.Db.Interfaces;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric;
using Microsoft.AspNetCore.Http;
using System;

namespace LT.DigitalOffice.WikiService.Mappers.Db
{
  public class DbRubricMapper : IDbRubricMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbRubricMapper(
      IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public DbRubric Map(CreateRubricRequest request)
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
  }
}
