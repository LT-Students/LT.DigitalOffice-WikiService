using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.WikiService.Mappers.Db.Interfaces;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;
using Microsoft.AspNetCore.Http;
using System;

namespace LT.DigitalOffice.WikiService.Mappers.Db
{
  public class DbArticleMapper : IDbArticleMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbArticleMapper(
      IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }
    public DbArticle Map(CreateArticleRequest request)
    {
      if (request is null)
      {
        return null;
      }

      return new DbArticle
      {
        Id = Guid.NewGuid(),
        Name = request.Name,
        Content = request.Content,
        RubricId = request.RubricId,
        IsActive = request.IsActive,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.UtcNow,
        ModifiedBy = request.IsActive ? _httpContextAccessor.HttpContext.GetUserId() : null,
        ModifiedAtUtc = request.IsActive ? DateTime.UtcNow : null
      };
    }
  }
}
