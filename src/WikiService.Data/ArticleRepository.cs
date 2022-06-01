using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Data
{
  public class ArticleRepository : IArticleRepository
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ArticleRepository(
      IDataProvider provider,
       IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> DoesSameNameExistAsync(Guid rubricId, string articleName)
    {
      return await _provider.Articles.AnyAsync(a => a.RubricId == rubricId && a.Name == articleName);
    }

    public async Task<Guid?> CreateAsync(DbArticle dbArticle)
    {
      if (dbArticle is null)
      {
        return null;
      }

      _provider.Articles.Add(dbArticle);
      await _provider.SaveAsync();

      return dbArticle.Id;
    }

    public async Task<DbArticle> GetAsync(Guid articleId)
    {
      return await _provider.Articles.Include(a => a.Files)
        .FirstOrDefaultAsync(article => article.Id == articleId);
    }

    public async Task<bool> EditAsync(Guid articleId, JsonPatchDocument<DbArticle> request)
    {
      DbArticle dbArticle = await _provider.Articles.FirstOrDefaultAsync(x => x.Id == articleId);

      if (dbArticle == null || request == null)
      {
        return false;
      }

      request.ApplyTo(dbArticle);
      dbArticle.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      dbArticle.ModifiedAtUtc = DateTime.UtcNow;
      await _provider.SaveAsync();

      return true;
    }
  }
}