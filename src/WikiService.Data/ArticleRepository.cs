using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;

namespace LT.DigitalOffice.WikiService.Data
{
  public class ArticleRepository : IArticleRepository
  {
    private readonly IDataProvider _provider;

    public ArticleRepository(
      IDataProvider provider)
    {
      _provider = provider;
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
  }
}