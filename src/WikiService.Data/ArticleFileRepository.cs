using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Data.Provider;

namespace LT.DigitalOffice.WikiService.Data
{
  public class ArticleFileRepository : IArticleFileRepository
  {
    private readonly IDataProvider _provider;

    public ArticleFileRepository(
      IDataProvider provider)
    {
      _provider = provider;
    }
  }
}