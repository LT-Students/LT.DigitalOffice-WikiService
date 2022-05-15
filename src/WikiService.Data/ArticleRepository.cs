using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

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

    public async Task<bool> DoesExistAsync(Guid rubricId)
    {
      return await _provider.Articles.AnyAsync(x => x.Id == rubricId);
    }

    public async Task<bool> DoesSameArticleNameExistAsync(Guid rubricId, string articleName)
    {
      return await _provider.Articles.AnyAsync(r => r.RubricId == rubricId && r.Name == articleName);
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
  }
}