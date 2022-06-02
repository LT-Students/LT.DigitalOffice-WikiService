using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Responses.Article;

namespace LT.DigitalOffice.WikiService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface IArticleResponseMapper
  {
    ArticleResponse Map(DbArticle dbArticle);
  }
}
