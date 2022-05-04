using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Data.Provider;

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
  }
}