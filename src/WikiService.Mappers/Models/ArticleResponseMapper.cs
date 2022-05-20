using System.Linq;
using LT.DigitalOffice.WikiService.Mappers.Models.Interfaces;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Responses.Article;

namespace LT.DigitalOffice.WikiService.Mappers.Models
{
  public class ArticleResponseMapper : IArticleResponseMapper
  {
    public ArticleResponse Map(DbArticle dbArticle)
    {
      if (dbArticle is null)
      {
        return null;
      }

      return new ArticleResponse
      {
        Id = dbArticle.Id,
        Name = dbArticle.Name,
        Content = dbArticle.Content,
        IsActive = dbArticle.IsActive,
        RubricId = dbArticle.RubricId,
        Files = dbArticle.Files
                ?.Select(f => f.FileId).ToList(),
      };
    }
  }
}
