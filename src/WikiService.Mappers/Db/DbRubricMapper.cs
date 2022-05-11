using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.WikiService.Mappers.Db.Interfaces;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
      if (request == null)
      {
        return null;
      }

      Guid rubricId = Guid.NewGuid();

      return new DbRubric
      {
        Id = rubricId,
        Name = request.Name.Trim(),
        ParentId = request.ParentId,
        IsActive = true,
        CreatedAtUtc = DateTime.UtcNow,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId()
      };
    }
  }
}
