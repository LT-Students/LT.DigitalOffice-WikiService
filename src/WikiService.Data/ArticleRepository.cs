using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
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