using LT.DigitalOffice.WikiService.Mappers.Models.Interfaces;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Models;

namespace LT.DigitalOffice.WikiService.Mappers.Models
{
  public class ArticleInfoMapper : IArticleInfoMapper
  {
    public ArticleInfo Map(DbArticle dbArticle)
    {
      if (dbArticle is null)
      {
        return null;
      }

      return new ArticleInfo
      {
        Id = dbArticle.Id,
        Name = dbArticle.Name
      };
    }
  }
}